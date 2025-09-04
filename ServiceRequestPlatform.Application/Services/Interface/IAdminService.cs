
using ServiceRequestPlatform.Application.DTOs.Admin;
using ServiceRequestPlatform.Application.DTOs.Customer;
using ServiceRequestPlatform.Application.DTOs.Worker;
using ServiceRequestPlatform.Application.DTOs.Service;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;

namespace ServiceRequestPlatform.Application.Services.Interface
{
    public interface IAdminService
    {
        Task<AdminDto> LoginAsync(AdminLoginDto dto);


        // Change password with ResetPasswordDto (fixed)
        Task ChangePasswordAsync(ResetPasswordDto dto);


        // Customers
        Task<IEnumerable<CustomerDto>> GetAllCustomersAsync();
        Task<CustomerDto> GetCustomerByIdAsync(int id);


        // Workers
        Task<IEnumerable<WorkerDto>> GetAllWorkersAsync();
        Task<WorkerDto> GetWorkerByIdAsync(int id);


        // Service Requests
        Task<IEnumerable<ServiceRequestDto>> GetAllServiceRequestsAsync();
        Task<ServiceRequestDto> GetServiceRequestByIdAsync(int id);
        Task AssignWorkerAsync(AssignWorkerDto dto);


        // Services
        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();
        Task<ServiceDto> GetServiceByIdAsync(int serviceId);
        Task<ServiceDto> AddServiceAsync(CreateServiceDto dto);
        Task UpdateServiceAsync(UpdateServiceDto dto);
        Task DeleteServiceAsync(int serviceId);
    }
}
