using ServiceRequestPlatform.Domain.Entities;

namespace ServiceRequestPlatform.Domain.Repositories.Interfaces
{
    public interface IRatingRepository : IRepository<Rating>
    {
        Task<IEnumerable<Rating>> GetRatingsByWorkerIdAsync(int workerId);
        Task<IEnumerable<Rating>> GetRatingsByCustomerIdAsync(int customerId);
        Task<IEnumerable<Rating>> GetRatingsByServiceRequestIdAsync(int serviceRequestId);
        Task<bool> HasCustomerRatedServiceRequestAsync(int customerId, int serviceRequestId);
        Task<double> GetAverageRatingForWorkerAsync(int workerId);
    }
}