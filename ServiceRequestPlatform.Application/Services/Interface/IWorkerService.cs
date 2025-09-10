using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceRequestPlatform.Application.DTOs.Worker;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;
using ServiceRequestPlatform.Application.DTOs.AvailabilitySlot;
using ServiceRequestPlatform.Application.DTOs.Rating;

namespace ServiceRequestPlatform.Application.Services.Interface
{
    public interface IWorkerService
    {
        
        Task<WorkerDto> RegisterAsync(RegisterWorkerDto dto);

       
        Task<WorkerDto> LoginAsync(WorkerLoginDto dto);

        Task<IEnumerable<ServiceRequestDto>> GetAssignedRequestsAsync(int workerId);

        Task<IEnumerable<AvailabilitySlotDto>> GetAvailabilityAsync(int workerId);

        
        Task<AvailabilitySlotDto> AddAvailabilityAsync(CreateAvailabilitySlotDto dto);

       
        Task UpdateAvailabilityAsync(UpdateAvailabilitySlotDto dto);
        Task DeleteAvailabilityAsync(int slotId);
        Task UpdateRequestStatusAsync(UpdateServiceRequestStatusDto dto);
        Task<WorkerDto> GetProfileAsync(int workerId);
        Task UpdateProfileAsync(UpdateWorkerDto dto);
        Task<IEnumerable<RatingDto>> GetMyRatingsAsync(int workerId);
        Task<double> GetAverageRatingAsync(int workerId);
    }
}
