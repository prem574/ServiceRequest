using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Application.DTOs.Worker
{
    public class WorkerDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
        public string Address { get; set; } = string.Empty;
        public string ServiceExpertise { get; set; } = string.Empty;
        public bool IsAvailable { get; set; }
        public List<int> ServiceIds { get; set; } = new();
    }
}
