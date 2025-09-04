using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.DTOs.ServiceRequest
{
    public class ServiceRequestDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int? WorkerId { get; set; }
        public int ServiceId { get; set; }

        public DateTime RequestedDate { get; set; }
        public TimeSpan RequestedTime { get; set; }
        public string Status { get; set; } = string.Empty;

        public string? Address { get; set; } = string.Empty;
        public int? AvailabilitySlotId { get; set; }
    }
}
