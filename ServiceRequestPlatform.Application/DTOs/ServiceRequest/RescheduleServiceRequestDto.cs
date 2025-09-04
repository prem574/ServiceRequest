using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.DTOs.ServiceRequest
{
    public class RescheduleServiceRequestDto
    {
        public int RequestId { get; set; }

        public DateTime NewDate { get; set; }
        public TimeSpan NewTime { get; set; }
    }
}
