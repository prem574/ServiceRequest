using Microsoft.EntityFrameworkCore;
using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Domain.Enums;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;
using ServiceRequestPlatform.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Infrastructure.Repositories.Implementations
{
    public class ServiceRequestRepository : GenericRepository<ServiceRequest>, IServiceRequestRepository
    {
        public ServiceRequestRepository(AppDbContext context) : base(context) { }


        public async Task<IEnumerable<ServiceRequest>> GetRequestsByCustomerIdAsync(int customerId)
        {
            return await _context.ServiceRequests
            .Where(r => r.CustomerId == customerId)
            .Include(r => r.Customer)
            .Include(r => r.Worker)
            .Include(r => r.Service)
            .ToListAsync();
        }


        public async Task<IEnumerable<ServiceRequest>> GetRequestsByWorkerIdAsync(int workerId)
        {
            return await _context.ServiceRequests
            .Where(r => r.WorkerId == workerId)
            .Include(r => r.Customer)
            .Include(r => r.Worker)
            .Include(r => r.Service)
            .ToListAsync();
        }


        public async Task<ServiceRequest?> GetWithDetailsAsync(int id)
        {
            return await _context.ServiceRequests
            .Include(r => r.Customer)
            .Include(r => r.Worker)
            .Include(r => r.Service)
            .Include(r => r.AvailabilitySlot)
            .FirstOrDefaultAsync(r => r.Id == id);
        }


        public async Task<IEnumerable<ServiceRequest>> GetRequestsByStatusAsync(ServiceRequestStatus status)
        {
            return await _context.ServiceRequests
            .Where(r => r.Status == status)
            .Include(r => r.Customer)
            .Include(r => r.Worker)
            .Include(r => r.Service)
            .ToListAsync();
        }


        public async Task<IEnumerable<ServiceRequest>> GetPendingRequestsAsync()
        {
            return await _context.ServiceRequests
            .Where(r => r.Status == ServiceRequestStatus.Pending)
            .Include(r => r.Customer)
            .Include(r => r.Service)
            .ToListAsync();
        }


        public async Task<IEnumerable<ServiceRequest>> GetRequestsByDateRangeAsync(DateTime startDate, DateTime endDate)
        {
            return await _context.ServiceRequests
            .Where(r => r.RequestedDate >= startDate && r.RequestedDate <= endDate)
            .Include(r => r.Customer)
            .Include(r => r.Worker)
            .Include(r => r.Service)
            .ToListAsync();
        }


        public async Task<int> GetRequestCountByCustomerAsync(int customerId)
        {
            return await _context.ServiceRequests.CountAsync(r => r.CustomerId == customerId);
        }


        public async Task<int> GetRequestCountByWorkerAsync(int workerId)
        {
            return await _context.ServiceRequests.CountAsync(r => r.WorkerId == workerId);
        }
    }
}
