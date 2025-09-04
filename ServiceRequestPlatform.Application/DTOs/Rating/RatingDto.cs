using System;

namespace ServiceRequestPlatform.Application.DTOs.Rating
{
    public class RatingDto
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; } // ADDED: Primary relationship
        public int CustomerId { get; set; }
        public int WorkerId { get; set; }
        public int Score { get; set; } // 1–5
        public string? Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        // ADDED: Navigation properties for display
        public string? CustomerName { get; set; }
        public string? WorkerName { get; set; }
        public string? ServiceName { get; set; }
    }
}
