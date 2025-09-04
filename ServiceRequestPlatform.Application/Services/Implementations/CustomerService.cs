using ServiceRequestPlatform.Application.DTOs.Customer;
using ServiceRequestPlatform.Application.DTOs.Rating;
using ServiceRequestPlatform.Application.DTOs.Service;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Domain.Enums;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;

namespace ServiceRequestPlatform.Application.Services.Implementations
{
    public class CustomerService : ICustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IPasswordService _passwordService;
        private readonly IAvailabilitySlotRepository _availabilitySlotRepository;

        public CustomerService(
            ICustomerRepository customerRepository,
            IServiceRepository serviceRepository,
            IServiceRequestRepository serviceRequestRepository,
            IRatingRepository ratingRepository,
            IPasswordService passwordService,
            IAvailabilitySlotRepository availabilitySlotRepository)
        {
            _customerRepository = customerRepository;
            _serviceRepository = serviceRepository;
            _serviceRequestRepository = serviceRequestRepository;
            _ratingRepository = ratingRepository;
            _passwordService = passwordService;
            _availabilitySlotRepository = availabilitySlotRepository;
        }

        public async Task<CustomerDto> RegisterAsync(CustomerRegisterDto dto)
        {
            var existingCustomer = await _customerRepository.GetByEmailAsync(dto.Email);
            if (existingCustomer != null) throw new InvalidOperationException("Email already registered");


            var customer = new Customer
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                CreatedAt = DateTime.UtcNow
            };


            await _customerRepository.AddAsync(customer);
            await _customerRepository.SaveAsync();


            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };
        }


        public async Task<CustomerDto> LoginAsync(CustomerLoginDto dto)
        {
            var customer = await _customerRepository.GetByEmailAsync(dto.Email);
            if (customer == null || !_passwordService.VerifyPassword(dto.Password, customer.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");


            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };
        }

        public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services.Where(s => s.IsActive).Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            });
        }

        public async Task<ServiceRequestDto> BookServiceAsync(BookServiceRequestDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.ServiceId);
            if (service == null || !service.IsActive)
                throw new ArgumentException("Service not found or inactive");

            AvailabilitySlot? slot = null;
            if (dto.AvailabilitySlotId.HasValue)
            {
                slot = await _availabilitySlotRepository.GetByIdAsync(dto.AvailabilitySlotId.Value);
                if (slot == null)
                    throw new ArgumentException("Availability slot not found");
                if (slot.IsBooked)
                    throw new InvalidOperationException("Selected slot is already booked");
            }

            var serviceRequest = new ServiceRequest
            {
                CustomerId = dto.CustomerId,
                ServiceId = dto.ServiceId,
                RequestedDate = slot?.AvailableDate ?? dto.RequestedDate,
                RequestedTime = slot?.StartTime ?? dto.RequestedTime,
                Address = dto.Address,
                Status = ServiceRequestStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                AvailabilitySlotId = slot?.Id,
                WorkerId = slot?.WorkerId
            };

            await _serviceRequestRepository.AddAsync(serviceRequest);

            // Mark slot as booked if one was selected
            if (slot != null)
            {
                slot.IsBooked = true;
                _availabilitySlotRepository.Update(slot);
            }

            // Save all changes in one transaction
            await _serviceRequestRepository.SaveAsync();

            return new ServiceRequestDto
            {
                Id = serviceRequest.Id,
                CustomerId = serviceRequest.CustomerId,
                ServiceId = serviceRequest.ServiceId,
                WorkerId = serviceRequest.WorkerId,
                RequestedDate = serviceRequest.RequestedDate,
                RequestedTime = serviceRequest.RequestedTime,
                Address = serviceRequest.Address,
                Status = serviceRequest.Status.ToString(),
                AvailabilitySlotId = serviceRequest.AvailabilitySlotId
            };
        }

        public async Task<ServiceRequestDto> RescheduleServiceAsync(RescheduleServiceRequestDto dto)
        {
            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(dto.RequestId);
            if (serviceRequest == null)
                throw new ArgumentException("Service request not found");

            if (serviceRequest.Status != ServiceRequestStatus.Pending && serviceRequest.Status != ServiceRequestStatus.Assigned)
                throw new InvalidOperationException("Cannot reschedule service request in current status");

            // Free up the old availability slot if one was booked
            if (serviceRequest.AvailabilitySlotId.HasValue)
            {
                var oldSlot = await _availabilitySlotRepository.GetByIdAsync(serviceRequest.AvailabilitySlotId.Value);
                if (oldSlot != null)
                {
                    oldSlot.IsBooked = false;
                    _availabilitySlotRepository.Update(oldSlot);
                }
            }

            serviceRequest.RequestedDate = dto.NewDate;
            serviceRequest.RequestedTime = dto.NewTime;
            serviceRequest.Status = ServiceRequestStatus.Rescheduled;
            serviceRequest.UpdatedAt = DateTime.UtcNow;
            serviceRequest.AvailabilitySlotId = null; // Clear the slot assignment

            _serviceRequestRepository.Update(serviceRequest);
            await _serviceRequestRepository.SaveAsync();

            return new ServiceRequestDto
            {
                Id = serviceRequest.Id,
                CustomerId = serviceRequest.CustomerId,
                WorkerId = serviceRequest.WorkerId,
                ServiceId = serviceRequest.ServiceId,
                RequestedDate = serviceRequest.RequestedDate,
                RequestedTime = serviceRequest.RequestedTime,
                Address = serviceRequest.Address,
                Status = serviceRequest.Status.ToString()
            };
        }

        public async Task CancelServiceAsync(int requestId)
        {
            var serviceRequest = await _serviceRequestRepository.GetByIdAsync(requestId);
            if (serviceRequest == null)
                throw new ArgumentException("Service request not found");

            if (serviceRequest.Status == ServiceRequestStatus.Completed)
                throw new InvalidOperationException("Cannot cancel completed service request");

            // Free up the availability slot if one was booked
            if (serviceRequest.AvailabilitySlotId.HasValue)
            {
                var slot = await _availabilitySlotRepository.GetByIdAsync(serviceRequest.AvailabilitySlotId.Value);
                if (slot != null)
                {
                    slot.IsBooked = false;
                    _availabilitySlotRepository.Update(slot);
                }
            }

            serviceRequest.Status = ServiceRequestStatus.Cancelled;
            serviceRequest.UpdatedAt = DateTime.UtcNow;

            _serviceRequestRepository.Update(serviceRequest);
            await _serviceRequestRepository.SaveAsync();
        }

        public async Task<RatingDto> RateServiceAsync(CreateRatingDto dto)
        {
            var serviceRequest = await _serviceRequestRepository.GetWithDetailsAsync(dto.ServiceRequestId);
            if (serviceRequest == null)
                throw new ArgumentException("Service request not found");

            if (serviceRequest.Status != ServiceRequestStatus.Completed)
                throw new InvalidOperationException("Can only rate completed service requests");

            // Check if rating already exists for this service request
            var existingRating = await _ratingRepository.GetAllAsync();
            if (existingRating.Any(r => r.ServiceRequestId == dto.ServiceRequestId))
                throw new InvalidOperationException("Service request has already been rated");

            var rating = new Rating
            {
                ServiceRequestId = dto.ServiceRequestId,
                CustomerId = dto.CustomerId,
                WorkerId = dto.WorkerId,
                Score = dto.Score,
                Comment = dto.Comment ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _ratingRepository.AddAsync(rating);
            await _ratingRepository.SaveAsync();

            return new RatingDto
            {
                Id = rating.Id,
                ServiceRequestId = rating.ServiceRequestId,
                CustomerId = rating.CustomerId,
                WorkerId = rating.WorkerId,
                Score = rating.Score,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt,
                CustomerName = serviceRequest.Customer?.FullName,
                WorkerName = serviceRequest.Worker?.FullName,
                ServiceName = serviceRequest.Service?.Name
            };
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetMyRequestsAsync(int customerId)
        {
            var requests = await _serviceRequestRepository.GetRequestsByCustomerIdAsync(customerId);
            return requests.Select(r => new ServiceRequestDto
            {
                Id = r.Id,
                CustomerId = r.CustomerId,
                WorkerId = r.WorkerId,
                ServiceId = r.ServiceId,
                RequestedDate = r.RequestedDate,
                RequestedTime = r.RequestedTime,
                Address = r.Address,
                Status = r.Status.ToString(),
                AvailabilitySlotId = r.AvailabilitySlotId
            });
        }

        public async Task<CustomerDto> GetProfileAsync(int customerId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer == null)
                throw new ArgumentException("Customer not found");

            return new CustomerDto
            {
                Id = customer.Id,
                FullName = customer.FullName,
                Email = customer.Email,
                PhoneNumber = customer.PhoneNumber,
                Address = customer.Address
            };
        }

        public async Task UpdateProfileAsync(CustomerDto dto)
        {
            var customer = await _customerRepository.GetByIdAsync(dto.Id);
            if (customer == null)
                throw new ArgumentException("Customer not found");

            customer.FullName = dto.FullName;
            customer.PhoneNumber = dto.PhoneNumber;
            customer.Address = dto.Address;

            _customerRepository.Update(customer);
            await _customerRepository.SaveAsync();
        }

        public async Task<IEnumerable<RatingDto>> GetMyRatingsAsync(int customerId)
        {
            var ratings = await _ratingRepository.GetAllAsync();
            var customerRatings = ratings.Where(r => r.CustomerId == customerId);

            return customerRatings.Select(r => new RatingDto
            {
                Id = r.Id,
                ServiceRequestId = r.ServiceRequestId,
                CustomerId = r.CustomerId,
                WorkerId = r.WorkerId,
                Score = r.Score,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                CustomerName = r.Customer?.FullName,
                WorkerName = r.Worker?.FullName,
                ServiceName = r.ServiceRequest?.Service?.Name
            });
        }
    }
}