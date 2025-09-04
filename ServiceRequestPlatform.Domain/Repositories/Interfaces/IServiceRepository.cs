using ServiceRequestPlatform.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Domain.Repositories.Interfaces
{
    public interface IServiceRepository : IRepository<Service>
    {
        Task<bool> IsServiceNameTakenAsync(string name);
    }
}
