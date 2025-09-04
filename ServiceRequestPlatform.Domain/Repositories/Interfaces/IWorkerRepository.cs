using ServiceRequestPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Domain.Repositories.Interfaces
{
    public interface IWorkerRepository : IRepository<Worker>
    {
        Task<IEnumerable<Worker>> GetAvailableWorkersByServiceIdAsync(int serviceId, DateTime date, TimeSpan time);
        Task<Worker?> GetByEmailAsync(string email); 
        Task<bool> IsEmailTakenAsync(string email);
        Task<IEnumerable<Worker>> GetAllWithServicesAsync();
    }
}
