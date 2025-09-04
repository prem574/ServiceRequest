using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.DTOs.ServiceRequest
{
    public class UpdateServiceRequestStatusDto
    {
        public int RequestId { get; set; }
        public DateTime NewRequestedDate { get; set; }
        public TimeSpan NewRequestedTime { get; set; }
        public string Status { get; set; } = string.Empty; // e.g., "Rescheduled", "Cancelled"
    }
}
