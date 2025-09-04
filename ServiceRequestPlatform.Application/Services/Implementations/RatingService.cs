using ServiceRequestPlatform.Application.DTOs.Rating;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.Services.Implementations
{
    public class RatingService : IRatingService
    {
        private readonly IRatingRepository _ratingRepository;
        private readonly IServiceRequestRepository _serviceRequestRepository;

        public RatingService(
            IRatingRepository ratingRepository,
            IServiceRequestRepository serviceRequestRepository)
        {
            _ratingRepository = ratingRepository;
            _serviceRequestRepository = serviceRequestRepository;
        }

        public async Task<RatingDto> CreateRatingAsync(CreateRatingDto dto)
        {
            var serviceRequest = await _serviceRequestRepository.GetWithDetailsAsync(dto.ServiceRequestId);
            if (serviceRequest == null)
                throw new ArgumentException("Service request not found");

            if (serviceRequest.Status != Domain.Enums.ServiceRequestStatus.Completed)
                throw new InvalidOperationException("Can only rate completed service requests");

            var rating = new Domain.Entities.Rating
            {
               
                ServiceRequestId = dto.ServiceRequestId,
                CustomerId = dto.CustomerId,
                WorkerId = dto.WorkerId,
                Score = dto.Score,
                Comment = dto.Comment ?? string.Empty,
                CreatedAt = DateTime.UtcNow
            };

            await _ratingRepository.AddAsync(rating);
            await _ratingRepository.SaveAsync();

            return new RatingDto
            {
                Id = rating.Id,
                ServiceRequestId = rating.ServiceRequestId,
                CustomerId = rating.CustomerId,
                WorkerId = rating.WorkerId,
                Score = rating.Score,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt,
                CustomerName = serviceRequest.Customer?.FullName,
                WorkerName = serviceRequest.Worker?.FullName,
                ServiceName = serviceRequest.Service?.Name
            };
        }

        public async Task<IEnumerable<RatingDto>> GetRatingsByWorkerAsync(int workerId)
        {
            var ratings = await _ratingRepository.GetRatingsByWorkerIdAsync(workerId);
            return ratings.Select(r => new RatingDto
            {
                Id = r.Id,
                ServiceRequestId = r.ServiceRequestId,
                CustomerId = r.CustomerId,
                WorkerId = r.WorkerId,
                Score = r.Score,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                CustomerName = r.Customer?.FullName,
                WorkerName = r.Worker?.FullName,
                ServiceName = r.ServiceRequest?.Service?.Name
            });
        }

        public async Task<IEnumerable<RatingDto>> GetRatingsByCustomerAsync(int customerId)
        {
            var allRatings = await _ratingRepository.GetAllAsync();
            var customerRatings = allRatings.Where(r => r.CustomerId == customerId);

            return customerRatings.Select(r => new RatingDto
            {
                Id = r.Id,
                ServiceRequestId = r.ServiceRequestId,
                CustomerId = r.CustomerId,
                WorkerId = r.WorkerId,
                Score = r.Score,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                CustomerName = r.Customer?.FullName,
                WorkerName = r.Worker?.FullName,
                ServiceName = r.ServiceRequest?.Service?.Name
            });
        }

        public async Task<IEnumerable<RatingDto>> GetRatingsByServiceRequestAsync(int serviceRequestId)
        {
            var allRatings = await _ratingRepository.GetAllAsync();
            var requestRatings = allRatings.Where(r => r.ServiceRequestId == serviceRequestId);

            return requestRatings.Select(r => new RatingDto
            {
                Id = r.Id,
                ServiceRequestId = r.ServiceRequestId,
                CustomerId = r.CustomerId,
                WorkerId = r.WorkerId,
                Score = r.Score,
                Comment = r.Comment,
                CreatedAt = r.CreatedAt,
                CustomerName = r.Customer?.FullName,
                WorkerName = r.Worker?.FullName,
                ServiceName = r.ServiceRequest?.Service?.Name
            });
        }

        public async Task<double> GetAverageRatingForWorkerAsync(int workerId)
        {
            var ratings = await _ratingRepository.GetRatingsByWorkerIdAsync(workerId);
            if (!ratings.Any())
                return 0.0;

            return ratings.Average(r => r.Score);
        }

        public async Task<RatingDto> GetRatingByIdAsync(int ratingId)
        {
            var rating = await _ratingRepository.GetByIdAsync(ratingId);
            if (rating == null)
                throw new ArgumentException("Rating not found");

            return new RatingDto
            {
                Id = rating.Id,
                ServiceRequestId = rating.ServiceRequestId,
                CustomerId = rating.CustomerId,
                WorkerId = rating.WorkerId,
                Score = rating.Score,
                Comment = rating.Comment,
                CreatedAt = rating.CreatedAt,
                CustomerName = rating.Customer?.FullName,
                WorkerName = rating.Worker?.FullName,
                ServiceName = rating.ServiceRequest?.Service?.Name
            };
        }
    }
}
