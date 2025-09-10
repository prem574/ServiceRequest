using System;


namespace ServiceRequestPlatform.Application.DTOs.AvailabilitySlot
{
    public class AvailabilitySlotDto
    {
        public int Id { get; set; }
        public int WorkerId { get; set; }

        public DateTime AvailableDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }

        public bool IsBooked { get; set; }
    }
}
