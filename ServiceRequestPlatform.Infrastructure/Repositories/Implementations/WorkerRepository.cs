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
    public class WorkerRepository : GenericRepository<Worker>, IWorkerRepository
    {
        public WorkerRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Worker>> GetAvailableWorkersByServiceIdAsync(int serviceId, DateTime date, TimeSpan time)
        {
            return await _context.Workers
                .Include(w => w.AvailabilitySlots)
                .Include(w => w.Services)
                .Where(w => w.Services.Any(s => s.Id == serviceId) && w.IsAvailable &&
                    w.AvailabilitySlots.Any(slot =>
                        slot.AvailableDate.Date == date.Date &&
                        slot.StartTime <= time && time < slot.EndTime && !slot.IsBooked))
                .ToListAsync();
        }

        public async Task<Worker?> GetByEmailAsync(string email)
        {
            return await _context.Workers
                .Include(w => w.Services)
                .FirstOrDefaultAsync(w => w.Email.ToLower() == email.ToLower());
        }

        public async Task<bool> IsEmailTakenAsync(string email)
        {
            return await _context.Workers.AnyAsync(w => w.Email.ToLower() == email.ToLower());
        }
        public async Task<IEnumerable<Worker>> GetAllWithServicesAsync()
        {
            return await _context.Workers.Include(w => w.Services).ToListAsync();
        }
    }
}