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
    public class ServiceRepository : GenericRepository<Service>, IServiceRepository
    {
        public ServiceRepository(AppDbContext context) : base(context) { }

        public async Task<bool> IsServiceNameTakenAsync(string name)
        {
            return await _context.Services.AnyAsync(s => s.Name.ToLower() == name.ToLower());
        }
    }
}
