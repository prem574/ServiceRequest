using ServiceRequestPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Domain.Repositories.Interfaces
{
    public interface IAvailabilitySlotRepository : IRepository<AvailabilitySlot>
    {
        Task<IEnumerable<AvailabilitySlot>> GetAvailableSlotsByWorkerIdAsync(int workerId);
        Task<bool> IsSlotAvailableAsync(int workerId, DateTime date, TimeSpan time);
    }
}
