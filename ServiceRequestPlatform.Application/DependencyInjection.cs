using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using ServiceRequestPlatform.Application.Services.Implementations;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Application.Validators.Customer_DTO_Validators;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;


namespace ServiceRequestPlatform.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            // Register validators from assembly
            services.AddValidatorsFromAssemblyContaining<CustomerDtoValidator>();

            // Application Services
            services.AddScoped<ICustomerService, CustomerService>();
            services.AddScoped<IWorkerService, WorkerService>();
            services.AddScoped<IAdminService, AdminService>();
            services.AddScoped<IServiceManagementService, ServiceManagementService>();
            services.AddScoped<IRatingService, RatingService>();
            services.AddScoped<IAvailabilityService, AvailabilityService>();
            services.AddScoped<IPasswordService, PasswordService>();
           


            return services;
        }
    }
}
