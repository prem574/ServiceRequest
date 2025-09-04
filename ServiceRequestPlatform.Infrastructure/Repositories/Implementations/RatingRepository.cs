using Microsoft.EntityFrameworkCore;
using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;
using ServiceRequestPlatform.Infrastructure.Data;

namespace ServiceRequestPlatform.Infrastructure.Repositories.Implementations
{
    public class RatingRepository : GenericRepository<Rating>, IRatingRepository
    {
        public RatingRepository(AppDbContext context) : base(context) { }

        public async Task<IEnumerable<Rating>> GetRatingsByWorkerIdAsync(int workerId)
        {
            return await _context.Ratings
                .Include(r => r.Customer)
                .Include(r => r.Worker)
                .Include(r => r.ServiceRequest)
                    .ThenInclude(sr => sr.Service)
                .Where(r => r.WorkerId == workerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByCustomerIdAsync(int customerId)
        {
            return await _context.Ratings
                .Include(r => r.Customer)
                .Include(r => r.Worker)
                .Include(r => r.ServiceRequest)
                    .ThenInclude(sr => sr.Service)
                .Where(r => r.CustomerId == customerId)
                .OrderByDescending(r => r.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Rating>> GetRatingsByServiceRequestIdAsync(int serviceRequestId)
        {
            return await _context.Ratings
                .Include(r => r.Customer)
                .Include(r => r.Worker)
                .Include(r => r.ServiceRequest)
                    .ThenInclude(sr => sr.Service)
                .Where(r => r.ServiceRequestId == serviceRequestId)
                .ToListAsync();
        }

        public async Task<bool> HasCustomerRatedServiceRequestAsync(int customerId, int serviceRequestId)
        {
            return await _context.Ratings
                .AnyAsync(r => r.CustomerId == customerId && r.ServiceRequestId == serviceRequestId);
        }

        public async Task<double> GetAverageRatingForWorkerAsync(int workerId)
        {
            var ratings = await _context.Ratings
                .Where(r => r.WorkerId == workerId)
                .ToListAsync();

            if (!ratings.Any())
                return 0.0;

            return ratings.Average(r => r.Score);
        }

        public override async Task<IEnumerable<Rating>> GetAllAsync()
        {
            return await _context.Ratings
                .Include(r => r.Customer)
                .Include(r => r.Worker)
                .Include(r => r.ServiceRequest)
                    .ThenInclude(sr => sr.Service)
                .ToListAsync();
        }

        public override async Task<Rating> GetByIdAsync(int id)
        {
            return await _context.Ratings
                .Include(r => r.Customer)
                .Include(r => r.Worker)
                .Include(r => r.ServiceRequest)
                    .ThenInclude(sr => sr.Service)
                .FirstOrDefaultAsync(r => r.Id == id);
        }
    }
}