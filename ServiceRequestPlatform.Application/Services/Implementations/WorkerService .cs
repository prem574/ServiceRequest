using ServiceRequestPlatform.Application.DTOs.AvailabilitySlot;
using ServiceRequestPlatform.Application.DTOs.Rating;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;
using ServiceRequestPlatform.Application.DTOs.Worker;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Domain.Enums;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;

namespace ServiceRequestPlatform.Application.Services.Implementations
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;
        private readonly IAvailabilitySlotRepository _availabilitySlotRepository;
        private readonly IRatingRepository _ratingRepository;
        private readonly IPasswordService _passwordService;

        public WorkerService(
            IWorkerRepository workerRepository,
            IServiceRequestRepository serviceRequestRepository,
            IAvailabilitySlotRepository availabilitySlotRepository,
            IRatingRepository ratingRepository,
            IPasswordService passwordService)
        {
            _workerRepository = workerRepository;
            _serviceRequestRepository = serviceRequestRepository;
            _availabilitySlotRepository = availabilitySlotRepository;
            _ratingRepository = ratingRepository;
            _passwordService = passwordService;
        }

        public async Task<WorkerDto> RegisterAsync(RegisterWorkerDto dto)
        {
            var existingWorker = await _workerRepository.GetByEmailAsync(dto.Email);
            if (existingWorker != null)
                throw new InvalidOperationException("Email already registered");

            var worker = new Worker
            {
                FullName = dto.FullName,
                Email = dto.Email,
                PasswordHash = _passwordService.HashPassword(dto.Password),
                PhoneNumber = dto.PhoneNumber,
                Address = dto.Address,
                ServiceExpertise = dto.ServiceExpertise,
                CreatedAt = DateTime.UtcNow
            };

            await _workerRepository.AddAsync(worker);
            await _workerRepository.SaveAsync();

            return new WorkerDto
            {
                Id = worker.Id,
                FullName = worker.FullName,
                Email = worker.Email,
                PhoneNumber = worker.PhoneNumber,
                Address = worker.Address,
                ServiceExpertise = worker.ServiceExpertise,
                IsAvailable = worker.IsAvailable
            };
        }

        public async Task<WorkerDto> LoginAsync(WorkerLoginDto dto)
        {
            var worker = await _workerRepository.GetByEmailAsync(dto.Email);
            if (worker == null || !_passwordService.VerifyPassword(dto.Password, worker.PasswordHash))
                throw new UnauthorizedAccessException("Invalid email or password");

            return new WorkerDto
            {
                Id = worker.Id,
                FullName = worker.FullName,
                Email = worker.Email,
                PhoneNumber = worker.PhoneNumber,
                Address = worker.Address,
                ServiceExpertise = worker.ServiceExpertise,
                IsAvailable = worker.IsAvailable
            };
        }

        public async Task<IEnumerable<ServiceRequestDto>> GetAssignedRequestsAsync(int workerId)
        {
            var requests = await _serviceRequestRepository.GetRequestsByWorkerIdAsync(workerId);
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

        public async Task<IEnumerable<AvailabilitySlotDto>> GetAvailabilityAsync(int workerId)
        {
            var slots = await _availabilitySlotRepository.GetAvailableSlotsByWorkerIdAsync(workerId);
            return slots.Select(s => new AvailabilitySlotDto
            {
                Id = s.Id,
                WorkerId = s.WorkerId,
                AvailableDate = s.AvailableDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsBooked = s.IsBooked
            });
        }

        public async Task<AvailabilitySlotDto> AddAvailabilityAsync(CreateAvailabilitySlotDto dto)
        {
            var worker = await _workerRepository.GetByIdAsync(dto.WorkerId);
            if (worker == null)
                throw new ArgumentException("Worker not found");

            // Check for overlapping slots
            var existingSlots = await _availabilitySlotRepository.GetAvailableSlotsByWorkerIdAsync(dto.WorkerId);
            var hasOverlap = existingSlots.Any(s =>
                s.AvailableDate.Date == dto.AvailableDate.Date &&
                ((dto.StartTime >= s.StartTime && dto.StartTime < s.EndTime) ||
                 (dto.EndTime > s.StartTime && dto.EndTime <= s.EndTime) ||
                 (dto.StartTime <= s.StartTime && dto.EndTime >= s.EndTime)));

            if (hasOverlap)
                throw new InvalidOperationException("Time slot overlaps with existing availability");

            var slot = new AvailabilitySlot
            {
                WorkerId = dto.WorkerId,
                AvailableDate = dto.AvailableDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsBooked = false
            };

            await _availabilitySlotRepository.AddAsync(slot);
            await _availabilitySlotRepository.SaveAsync();

            return new AvailabilitySlotDto
            {
                Id = slot.Id,
                WorkerId = slot.WorkerId,
                AvailableDate = slot.AvailableDate,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsBooked = slot.IsBooked
            };
        }

        public async Task UpdateAvailabilityAsync(UpdateAvailabilitySlotDto dto)
        {
            var slot = await _availabilitySlotRepository.GetByIdAsync(dto.Id);
            if (slot == null)
                throw new ArgumentException("Availability slot not found");

            if (slot.IsBooked)
                throw new InvalidOperationException("Cannot update booked availability slot");

            // Check for overlapping slots (excluding current slot)
            var existingSlots = await _availabilitySlotRepository.GetAvailableSlotsByWorkerIdAsync(slot.WorkerId);
            var hasOverlap = existingSlots.Where(s => s.Id != dto.Id).Any(s =>
                s.AvailableDate.Date == dto.AvailableDate.Date &&
                ((dto.StartTime >= s.StartTime && dto.StartTime < s.EndTime) ||
                 (dto.EndTime > s.StartTime && dto.EndTime <= s.EndTime) ||
                 (dto.StartTime <= s.StartTime && dto.EndTime >= s.EndTime)));

            if (hasOverlap)
                throw new InvalidOperationException("Time slot overlaps with existing availability");

            slot.AvailableDate = dto.AvailableDate;
            slot.StartTime = dto.StartTime;
            slot.EndTime = dto.EndTime;

            _availabilitySlotRepository.Update(slot);
            await _availabilitySlotRepository.SaveAsync();
        }

        public async Task DeleteAvailabilityAsync(int slotId)
        {
            var slot = await _availabilitySlotRepository.GetByIdAsync(slotId);
            if (slot == null)
                throw new ArgumentException("Availability slot not found");

            if (slot.IsBooked)
                throw new InvalidOperationException("Cannot delete booked availability slot");

            _availabilitySlotRepository.Delete(slot);
            await _availabilitySlotRepository.SaveAsync();
        }

        public async Task UpdateRequestStatusAsync(UpdateServiceRequestStatusDto dto)
        {
            var request = await _serviceRequestRepository.GetByIdAsync(dto.RequestId);
            if (request == null)
                throw new ArgumentException("Service request not found");

            if (!Enum.TryParse<ServiceRequestStatus>(dto.Status, out var status))
            {
                throw new ArgumentException("Invalid status value");
            }

            // Validate status transitions
            var validTransitions = GetValidStatusTransitions(request.Status);
            if (!validTransitions.Contains(status))
            {
                throw new InvalidOperationException($"Cannot change status from {request.Status} to {status}");
            }

            request.Status = status;

            // Handle rescheduling
            if (status == ServiceRequestStatus.Rescheduled)
            {
                request.RequestedDate = dto.NewRequestedDate;
                request.RequestedTime = dto.NewRequestedTime;
            }

            // Free up availability slot for cancelled/completed requests
            if ((status == ServiceRequestStatus.Cancelled || status == ServiceRequestStatus.Completed)
                && request.AvailabilitySlotId.HasValue)
            {
                var slot = await _availabilitySlotRepository.GetByIdAsync(request.AvailabilitySlotId.Value);
                if (slot != null)
                {
                    slot.IsBooked = false;
                    _availabilitySlotRepository.Update(slot);
                }
            }

            request.UpdatedAt = DateTime.UtcNow;
            _serviceRequestRepository.Update(request);
            await _serviceRequestRepository.SaveAsync();
        }

        private static List<ServiceRequestStatus> GetValidStatusTransitions(ServiceRequestStatus currentStatus)
        {
            return currentStatus switch
            {
                ServiceRequestStatus.Pending => new List<ServiceRequestStatus>
                {
                    ServiceRequestStatus.Assigned,
                    ServiceRequestStatus.Cancelled,
                    ServiceRequestStatus.Rescheduled
                },
                ServiceRequestStatus.Assigned => new List<ServiceRequestStatus>
                {
                    ServiceRequestStatus.InProgress,
                    ServiceRequestStatus.Cancelled,
                    ServiceRequestStatus.Rescheduled
                },
                ServiceRequestStatus.InProgress => new List<ServiceRequestStatus>
                {
                    ServiceRequestStatus.Completed,
                    ServiceRequestStatus.Cancelled
                },
                ServiceRequestStatus.Rescheduled => new List<ServiceRequestStatus>
                {
                    ServiceRequestStatus.Assigned,
                    ServiceRequestStatus.Cancelled
                },
                ServiceRequestStatus.Completed => new List<ServiceRequestStatus>(), // No further transitions
                ServiceRequestStatus.Cancelled => new List<ServiceRequestStatus>(), // No further transitions
                _ => new List<ServiceRequestStatus>()
            };
        }

        public async Task<WorkerDto> GetProfileAsync(int workerId)
        {
            var worker = await _workerRepository.GetByIdAsync(workerId);
            if (worker == null)
                throw new ArgumentException("Worker not found");

            return new WorkerDto
            {
                Id = worker.Id,
                FullName = worker.FullName,
                Email = worker.Email,
                PhoneNumber = worker.PhoneNumber,
                Address = worker.Address,
                ServiceExpertise = worker.ServiceExpertise,
                IsAvailable = worker.IsAvailable
            };
        }

        public async Task UpdateProfileAsync(UpdateWorkerDto dto)
        {
            var worker = await _workerRepository.GetByIdAsync(dto.WorkerId);
            if (worker == null)
                throw new ArgumentException("Worker not found");

            worker.FullName = dto.FullName;
            worker.PhoneNumber = dto.PhoneNumber;
            worker.Address = dto.Address;
            worker.ServiceExpertise = dto.ServiceExpertise;

            _workerRepository.Update(worker);
            await _workerRepository.SaveAsync();
        }

        public async Task<IEnumerable<RatingDto>> GetMyRatingsAsync(int workerId)
        {
            var ratings = await _ratingRepository.GetRatingsByWorkerIdAsync(workerId);
            return ratings.Select(r => new RatingDto
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

        public async Task<double> GetAverageRatingAsync(int workerId)
        {
            return await _ratingRepository.GetAverageRatingForWorkerAsync(workerId);
        }
    }
}