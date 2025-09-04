using ServiceRequestPlatform.Application.DTOs.Admin;
using ServiceRequestPlatform.Application.DTOs.Customer;
using ServiceRequestPlatform.Application.DTOs.Service;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;
using ServiceRequestPlatform.Application.DTOs.Worker;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Domain.Enums;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;


namespace ServiceRequestPlatform.Application.Services.Implementations
{
    public class AdminService : IAdminService
    {
        private readonly IAdminRepository _adminRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IWorkerRepository _workerRepository;
        private readonly IServiceRepository _serviceRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IPasswordService _passwordService;

        public AdminService(
            IAdminRepository adminRepository,
            ICustomerRepository customerRepository,
            IWorkerRepository workerRepository,
            IServiceRepository serviceRepository,
            IServiceRequestRepository serviceRequestRepository,
            IPasswordService passwordService)
        {
            _adminRepository = adminRepository;
            _customerRepository = customerRepository;
            _workerRepository = workerRepository;
            _serviceRepository = serviceRepository;
            _serviceRequestRepository = serviceRequestRepository;
            _passwordService = passwordService;
        }

        public async Task<AdminDto> LoginAsync(AdminLoginDto dto)
        {
            var admin = await _adminRepository.GetByEmailAsync(dto.Email);
            if (admin == null || !_passwordService.VerifyPassword(dto.Password, admin.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            return new AdminDto { Id = admin.Id, FullName = admin.FullName, Email = admin.Email };
        }

        public async Task<ServiceDto> GetServiceByIdAsync(int serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId) ?? throw new ArgumentException("Service not found");
            return new ServiceDto { Id = service.Id, Name = service.Name, Description = service.Description };
        }

        public async Task AssignWorkerAsync(AssignWorkerDto dto)
        {
            var request = await _serviceRequestRepository.GetByIdAsync(dto.RequestId) ?? throw new ArgumentException("Service request not found");
            var worker = await _workerRepository.GetByIdAsync(dto.WorkerId) ?? throw new ArgumentException("Worker not found");

            request.WorkerId = dto.WorkerId;
            request.Status = ServiceRequestStatus.Assigned;
            request.UpdatedAt = DateTime.UtcNow;

            _serviceRequestRepository.Update(request);
            await _serviceRequestRepository.SaveAsync();
        }

        // Fixed: Now matches IAdminService.ResetPasswordDto signature
        public async Task ChangePasswordAsync(ResetPasswordDto dto)
        {
            var admin = await _adminRepository.GetByIdAsync(dto.AdminId) ?? throw new ArgumentException("Admin not found");
            if (!_passwordService.VerifyPassword(dto.CurrentPassword, admin.PasswordHash))
                throw new UnauthorizedAccessException("Current password is incorrect");
            if (dto.NewPassword != dto.ConfirmPassword)
                throw new ArgumentException("Passwords do not match");

            admin.PasswordHash = _passwordService.HashPassword(dto.NewPassword);
            _adminRepository.Update(admin);
            await _adminRepository.SaveAsync();
        }

        public async Task<IEnumerable<CustomerDto>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            return customers.Select(c => new CustomerDto
            {
                Id = c.Id,
                FullName = c.FullName,
                Email = c.Email,
                PhoneNumber = c.PhoneNumber,
                Address = c.Address
            });
        }

        public async Task<CustomerDto> GetCustomerByIdAsync(int id)
        {
            var c = await _customerRepository.GetByIdAsync(id) ?? throw new ArgumentException("Customer not found");
            return new CustomerDto { Id = c.Id, FullName = c.FullName, Email = c.Email, PhoneNumber = c.PhoneNumber, Address = c.Address };
        }

        public async Task<IEnumerable<WorkerDto>> GetAllWorkersAsync()
        {
            var workers = await _workerRepository.GetAllWithServicesAsync();
            return workers.Select(w => new WorkerDto
            {
                Id = w.Id,
                FullName = w.FullName,
                Email = w.Email,
                PhoneNumber = w.PhoneNumber,
                Address = w.Address,
                ServiceExpertise = w.ServiceExpertise,
                IsAvailable = w.IsAvailable,
                ServiceIds = w.Services.Select(s => s.Id).ToList()
            });
        }

        public async Task<WorkerDto> GetWorkerByIdAsync(int id)
        {
            var w = await _workerRepository.GetByIdAsync(id) ?? throw new ArgumentException("Worker not found");
            return new WorkerDto
            {
                Id = w.Id,
                FullName = w.FullName,
                Email = w.Email,
                PhoneNumber = w.PhoneNumber,
                Address = w.Address,
                ServiceExpertise = w.ServiceExpertise,
                IsAvailable = w.IsAvailable,
                ServiceIds = w.Services.Select(s => s.Id).ToList()
            };
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetAllServiceRequestsAsync()
        {
            var requests = await _serviceRequestRepository.GetAllAsync();
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

        public async Task<ServiceRequestDto> GetServiceRequestByIdAsync(int id)
        {
            var r = await _serviceRequestRepository.GetByIdAsync(id) ?? throw new ArgumentException("Service request not found");
            return new ServiceRequestDto
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
            };
        }

        public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services.Select(s => new ServiceDto { Id = s.Id, Name = s.Name, Description = s.Description });
        }

        public async Task<ServiceDto> AddServiceAsync(CreateServiceDto dto)
        {
            var isNameTaken = await _serviceRepository.IsServiceNameTakenAsync(dto.Name);
            if (isNameTaken) throw new ArgumentException("Service name already exists");

            var service = new ServiceRequestPlatform.Domain.Entities.Service
            {
                Name = dto.Name,
                Description = dto.Description,
                Duration = TimeSpan.FromHours(1),
                Price = 0,
                CreatedAt = DateTime.UtcNow
            };
            await _serviceRepository.AddAsync(service);
            await _serviceRepository.SaveAsync();
            return new ServiceDto { Id = service.Id, Name = service.Name, Description = service.Description };
        }

        public async Task UpdateServiceAsync(UpdateServiceDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.Id) ?? throw new ArgumentException("Service not found");
            var isNameTaken = await _serviceRepository.IsServiceNameTakenAsync(dto.Name);
            if (isNameTaken && !string.Equals(service.Name, dto.Name, StringComparison.OrdinalIgnoreCase))
                throw new ArgumentException("Service name already exists");
            service.Name = dto.Name;
            service.Description = dto.Description;
            _serviceRepository.Update(service);
            await _serviceRepository.SaveAsync();
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId) ?? throw new ArgumentException("Service not found");
            service.IsActive = false;
            _serviceRepository.Update(service);
            await _serviceRepository.SaveAsync();
        }
    }

}
