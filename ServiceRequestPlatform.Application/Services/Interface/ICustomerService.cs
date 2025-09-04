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

        // FIXED: LoginDto is correct for customer login
        Task<CustomerDto> LoginAsync(CustomerLoginDto dto);

        Task<IEnumerable<ServiceDto>> GetAllServicesAsync();

        // FIXED: Changed BookServiceDto to BookServiceRequestDto
        Task<ServiceRequestDto> BookServiceAsync(BookServiceRequestDto dto);

        // FIXED: Changed to use RescheduleServiceRequestDto
        Task<ServiceRequestDto> RescheduleServiceAsync(RescheduleServiceRequestDto dto);

        Task CancelServiceAsync(int requestId);

        // FIXED: Changed RatingDto to CreateRatingDto for creating ratings
        Task<RatingDto> RateServiceAsync(CreateRatingDto dto);

        Task<IEnumerable<ServiceRequestDto>> GetMyRequestsAsync(int customerId);

        // ADDED: Additional useful methods for customers
        Task<CustomerDto> GetProfileAsync(int customerId);
        Task UpdateProfileAsync(CustomerDto dto);
        Task<IEnumerable<RatingDto>> GetMyRatingsAsync(int customerId);
    }
}
