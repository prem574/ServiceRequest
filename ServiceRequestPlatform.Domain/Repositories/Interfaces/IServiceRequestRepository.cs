using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Domain.Enums;

namespace ServiceRequestPlatform.Domain.Repositories.Interfaces
{
    public interface IServiceRequestRepository : IRepository<ServiceRequest>
    {
        Task<IEnumerable<ServiceRequest>> GetRequestsByCustomerIdAsync(int customerId);
        Task<IEnumerable<ServiceRequest>> GetRequestsByWorkerIdAsync(int workerId);
        Task<ServiceRequest?> GetWithDetailsAsync(int id);
        Task<IEnumerable<ServiceRequest>> GetRequestsByStatusAsync(ServiceRequestStatus status);
        Task<IEnumerable<ServiceRequest>> GetPendingRequestsAsync();
        Task<IEnumerable<ServiceRequest>> GetRequestsByDateRangeAsync(DateTime startDate, DateTime endDate);
        Task<int> GetRequestCountByCustomerAsync(int customerId);
        Task<int> GetRequestCountByWorkerAsync(int workerId);
    }
}