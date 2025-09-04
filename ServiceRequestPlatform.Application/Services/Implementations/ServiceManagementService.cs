using ServiceRequestPlatform.Application.DTOs.Service;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;


namespace ServiceRequestPlatform.Application.Services.Implementations
{
    public class ServiceManagementService : IServiceManagementService
    {
        private readonly IServiceRepository _serviceRepository;

        public ServiceManagementService(IServiceRepository serviceRepository)
        {
            _serviceRepository = serviceRepository;
        }

        public async Task<IEnumerable<ServiceDto>> GetAllServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            return services.Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            });
        }

        public async Task<ServiceDto> GetServiceByIdAsync(int serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
                throw new ArgumentException("Service not found");

            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description
            };
        }

        public async Task<ServiceDto> CreateServiceAsync(CreateServiceDto dto)
        {
            var isNameTaken = await _serviceRepository.IsServiceNameTakenAsync(dto.Name);
            if (isNameTaken)
                throw new ArgumentException("Service name already exists");

            var service = new ServiceRequestPlatform.Domain.Entities.Service
            {
               
                Name = dto.Name,
                Description = dto.Description,
                Duration = TimeSpan.FromHours(1), // Default duration
                Price = 0, // Default price
                CreatedAt = DateTime.UtcNow
            };

            await _serviceRepository.AddAsync(service);
            await _serviceRepository.SaveAsync();

            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description
            };
        }

        public async Task<ServiceDto> UpdateServiceAsync(UpdateServiceDto dto)
        {
            var service = await _serviceRepository.GetByIdAsync(dto.Id);
            if (service == null)
                throw new ArgumentException("Service not found");

            // Check if name is taken by another service
            var existingService = await _serviceRepository.GetAllAsync();
            var conflictingService = existingService.FirstOrDefault(s => s.Name.ToLower() == dto.Name.ToLower() && s.Id != dto.Id);
            if (conflictingService != null)
                throw new ArgumentException("Service name already exists");

            service.Name = dto.Name;
            service.Description = dto.Description;

            _serviceRepository.Update(service);
            await _serviceRepository.SaveAsync();

            return new ServiceDto
            {
                Id = service.Id,
                Name = service.Name,
                Description = service.Description
            };
        }

        public async Task DeleteServiceAsync(int serviceId)
        {
            var service = await _serviceRepository.GetByIdAsync(serviceId);
            if (service == null)
                throw new ArgumentException("Service not found");

            // Soft delete - mark as inactive instead of hard delete
            service.IsActive = false;
            _serviceRepository.Update(service);
            await _serviceRepository.SaveAsync();
        }

        public async Task<IEnumerable<ServiceDto>> GetActiveServicesAsync()
        {
            var services = await _serviceRepository.GetAllAsync();
            var activeServices = services.Where(s => s.IsActive);

            return activeServices.Select(s => new ServiceDto
            {
                Id = s.Id,
                Name = s.Name,
                Description = s.Description
            });
        }
    }
}
