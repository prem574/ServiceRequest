using ServiceRequestPlatform.Domain.Enums;
using System;
using System.Collections.Generic;

namespace ServiceRequestPlatform.Domain.Entities
{
    public class ServiceRequest
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public Customer? Customer { get; set; }
        public int? WorkerId { get; set; }
        public Worker? Worker { get; set; }
        public int ServiceId { get; set; }
        public Service? Service { get; set; }
        public DateTime RequestedDate { get; set; }
        public TimeSpan RequestedTime { get; set; }
        public string Address { get; set; } = string.Empty;
        public ServiceRequestStatus Status { get; set; } = ServiceRequestStatus.Pending;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }
        public int? AvailabilitySlotId { get; set; }
        public AvailabilitySlot? AvailabilitySlot { get; set; }
    }
}
