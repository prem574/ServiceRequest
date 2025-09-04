using System;

namespace ServiceRequestPlatform.Application.DTOs.Rating
{
    public class CreateRatingDto
    {
        public int ServiceRequestId { get; set; } // ADDED: Primary relationship
        public int CustomerId { get; set; } // KEPT: For validation
        public int WorkerId { get; set; } // KEPT: For validation
        public int Score { get; set; }
        public string? Comment { get; set; }
    }
}