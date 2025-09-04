using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceRequestPlatform.Application.DTOs.Service;

namespace ServiceRequestPlatform.Application.Services.Interface
{
    public interface IServiceManagementService
    {
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto> GetServiceByIdAsync(int serviceId);
        Task<ServiceDto> CreateServiceAsync(CreateServiceDto dto);
        Task<ServiceDto> UpdateServiceAsync(UpdateServiceDto dto);
        Task DeleteServiceAsync(int serviceId);
        Task<IEnumerable<ServiceDto>> GetActiveServicesAsync();
    }
}