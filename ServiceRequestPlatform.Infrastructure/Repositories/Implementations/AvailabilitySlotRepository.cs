using Microsoft.EntityFrameworkCore;
using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;
using ServiceRequestPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Infrastructure.Repositories.Implementations
{
    public class AvailabilitySlotRepository : GenericRepository<AvailabilitySlot>, IAvailabilitySlotRepository
    {
        public AvailabilitySlotRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<AvailabilitySlot>> GetAvailableSlotsByWorkerIdAsync(int workerId)
        {
            return await _context.AvailabilitySlots
                .Where(s => s.WorkerId == workerId && !s.IsBooked)
                .ToListAsync();
        }

        public async Task<bool> IsSlotAvailableAsync(int workerId, DateTime date, TimeSpan time)
        {
            return await _context.AvailabilitySlots.AnyAsync(s =>
                s.WorkerId == workerId &&
                s.AvailableDate.Date == date.Date &&
                s.StartTime <= time && time < s.EndTime &&
                !s.IsBooked);
        }
    }
}
