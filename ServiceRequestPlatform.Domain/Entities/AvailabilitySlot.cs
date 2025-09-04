using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServiceRequestPlatform.Domain.Entities
{
    public class AvailabilitySlot
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }
        public Worker? Worker { get; set; }
        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
        public bool IsBooked { get; set; } = false;
    }
}
