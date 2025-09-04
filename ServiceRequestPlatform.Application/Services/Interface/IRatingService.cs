using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ServiceRequestPlatform.Application.DTOs.Rating;

namespace ServiceRequestPlatform.Application.Services.Interface
{
    public interface IRatingService
    {
        Task<RatingDto> CreateRatingAsync(CreateRatingDto dto);
        Task<IEnumerable<RatingDto>> GetRatingsByWorkerAsync(int workerId);
        Task<IEnumerable<RatingDto>> GetRatingsByCustomerAsync(int customerId);
        Task<IEnumerable<RatingDto>> GetRatingsByServiceRequestAsync(int serviceRequestId);
        Task<double> GetAverageRatingForWorkerAsync(int workerId);
        Task<RatingDto> GetRatingByIdAsync(int ratingId);
    }
}