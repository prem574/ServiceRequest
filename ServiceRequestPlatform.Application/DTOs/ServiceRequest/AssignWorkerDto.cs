using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.DTOs.ServiceRequest
{
    public class AssignWorkerDto
    {
        public int RequestId { get; set; }
        public int WorkerId { get; set; }
    }
}
