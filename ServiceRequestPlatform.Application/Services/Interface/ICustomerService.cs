using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceRequestPlatform.Application.DTOs.Customer;
using ServiceRequestPlatform.Application.DTOs.Service;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;
using ServiceRequestPlatform.Application.DTOs.Rating;

namespace ServiceRequestPlatform.Application.Services.Interface
{
    public interface ICustomerService
    {
        Task<CustomerDto> RegisterAsync(CustomerRegisterDto dto);

      
        Task<CustomerDto> LoginAsync(CustomerLoginDto dto);

        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();

      
        Task<ServiceRequestDto> BookServiceAsync(BookServiceRequestDto dto);

        
        Task<ServiceRequestDto> RescheduleServiceAsync(RescheduleServiceRequestDto dto);

        Task CancelServiceAsync(int requestId);

        
        Task<RatingDto> RateServiceAsync(CreateRatingDto dto);

        Task<IEnumerable<ServiceRequestDto>> GetMyRequestsAsync(int customerId);

     
        Task<CustomerDto> GetProfileAsync(int customerId);
        Task UpdateProfileAsync(CustomerDto dto);
        Task<IEnumerable<RatingDto>> GetMyRatingsAsync(int customerId);
    }
}
