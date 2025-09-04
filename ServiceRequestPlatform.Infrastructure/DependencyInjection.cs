using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;
using ServiceRequestPlatform.Infrastructure.Data;
using ServiceRequestPlatform.Infrastructure.Repositories.Implementations;


namespace ServiceRequestPlatform.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            // Database
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // Repositories
            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IWorkerRepository, WorkerRepository>();
            services.AddScoped<IServiceRepository, ServiceRepository>();
            services.AddScoped<IServiceRequestRepository, ServiceRequestRepository>();
            services.AddScoped<IAvailabilitySlotRepository, AvailabilitySlotRepository>();
            services.AddScoped<IRatingRepository, RatingRepository>();
            services.AddScoped<IAdminRepository, AdminRepository>();

            // Infrastructure Services - use Application's IPasswordService
            //services.AddScoped<IPasswordService, PasswordService>();

            return services;
        }
    }
}