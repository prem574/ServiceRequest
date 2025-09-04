using System;

namespace ServiceRequestPlatform.Domain.Entities
{
    public class Rating
    {
        public int Id { get; set; }
        public int ServiceRequestId { get; set; }
        public ServiceRequest? ServiceRequest { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }
        public int Score { get; set; }
        public string Comment { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}