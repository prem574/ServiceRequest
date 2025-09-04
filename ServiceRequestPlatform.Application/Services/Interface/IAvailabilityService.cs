using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceRequestPlatform.Application.DTOs.AvailabilitySlot;

namespace ServiceRequestPlatform.Application.Services.Interface
{
    public interface IAvailabilityService
    {
        Task<IEnumerable<AvailabilitySlotDto>> GetWorkerAvailabilityAsync(int workerId);
        Task<IEnumerable<AvailabilitySlotDto>> GetAvailableSlotsByDateAsync(DateTime date);
        Task<AvailabilitySlotDto> CreateAvailabilitySlotAsync(CreateAvailabilitySlotDto dto);
        Task<AvailabilitySlotDto> UpdateAvailabilitySlotAsync(UpdateAvailabilitySlotDto dto);
        Task DeleteAvailabilitySlotAsync(int slotId);
        Task<bool> IsSlotAvailableAsync(int slotId);
        Task MarkSlotAsBookedAsync(int slotId);
        Task MarkSlotAsAvailableAsync(int slotId);
    }
}