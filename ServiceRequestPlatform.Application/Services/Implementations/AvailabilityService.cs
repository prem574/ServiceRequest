using ServiceRequestPlatform.Application.DTOs.AvailabilitySlot;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Domain.Repositories.Interfaces;


namespace ServiceRequestPlatform.Application.Services.Implementations
{
    public class AvailabilityService : IAvailabilityService
    {
        private readonly IAvailabilitySlotRepository _availabilitySlotRepository;
        private readonly IWorkerRepository _workerRepository;

        public AvailabilityService(
            IAvailabilitySlotRepository availabilitySlotRepository,
            IWorkerRepository workerRepository)
        {
            _availabilitySlotRepository = availabilitySlotRepository;
            _workerRepository = workerRepository;
        }

        public async Task<IEnumerable<AvailabilitySlotDto>> GetWorkerAvailabilityAsync(int workerId)
        {
            var slots = await _availabilitySlotRepository.GetAvailableSlotsByWorkerIdAsync(workerId);
            return slots.Select(s => new AvailabilitySlotDto
            {
                Id = s.Id,
                WorkerId = s.WorkerId,
                AvailableDate = s.AvailableDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsBooked = s.IsBooked
            });
        }

        public async Task<IEnumerable<AvailabilitySlotDto>> GetAvailableSlotsByDateAsync(DateTime date)
        {
            var allSlots = await _availabilitySlotRepository.GetAllAsync();
            var dateSlots = allSlots.Where(s => s.AvailableDate.Date == date.Date && !s.IsBooked);

            return dateSlots.Select(s => new AvailabilitySlotDto
            {
                Id = s.Id,
                WorkerId = s.WorkerId,
                AvailableDate = s.AvailableDate,
                StartTime = s.StartTime,
                EndTime = s.EndTime,
                IsBooked = s.IsBooked
            });
        }

        public async Task<AvailabilitySlotDto> CreateAvailabilitySlotAsync(CreateAvailabilitySlotDto dto)
        {
            var worker = await _workerRepository.GetByIdAsync(dto.WorkerId);
            if (worker == null)
                throw new ArgumentException("Worker not found");

            var slot = new AvailabilitySlot
            {
               
                WorkerId = dto.WorkerId,
                AvailableDate = dto.AvailableDate,
                StartTime = dto.StartTime,
                EndTime = dto.EndTime,
                IsBooked = false
            };

            await _availabilitySlotRepository.AddAsync(slot);
            await _availabilitySlotRepository.SaveAsync();

            return new AvailabilitySlotDto
            {
                Id = slot.Id,
                WorkerId = slot.WorkerId,
                AvailableDate = slot.AvailableDate,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsBooked = slot.IsBooked
            };
        }

        public async Task<AvailabilitySlotDto> UpdateAvailabilitySlotAsync(UpdateAvailabilitySlotDto dto)
        {
            var slot = await _availabilitySlotRepository.GetByIdAsync(dto.Id);
            if (slot == null)
                throw new ArgumentException("Availability slot not found");

            if (slot.IsBooked)
                throw new InvalidOperationException("Cannot update booked availability slot");

            slot.AvailableDate = dto.AvailableDate;
            slot.StartTime = dto.StartTime;
            slot.EndTime = dto.EndTime;

            _availabilitySlotRepository.Update(slot);
            await _availabilitySlotRepository.SaveAsync();

            return new AvailabilitySlotDto
            {
                Id = slot.Id,
                WorkerId = slot.WorkerId,
                AvailableDate = slot.AvailableDate,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                IsBooked = slot.IsBooked
            };
        }

        public async Task DeleteAvailabilitySlotAsync(int slotId)
        {
            var slot = await _availabilitySlotRepository.GetByIdAsync(slotId);
            if (slot == null)
                throw new ArgumentException("Availability slot not found");

            if (slot.IsBooked)
                throw new InvalidOperationException("Cannot delete booked availability slot");

            _availabilitySlotRepository.Delete(slot);
            await _availabilitySlotRepository.SaveAsync();
        }

        public async Task<bool> IsSlotAvailableAsync(int slotId)
        {
            var slot = await _availabilitySlotRepository.GetByIdAsync(slotId);
            return slot != null && !slot.IsBooked;
        }

        public async Task MarkSlotAsBookedAsync(int slotId)
        {
            var slot = await _availabilitySlotRepository.GetByIdAsync(slotId);
            if (slot == null)
                throw new ArgumentException("Availability slot not found");

            slot.IsBooked = true;
            _availabilitySlotRepository.Update(slot);
            await _availabilitySlotRepository.SaveAsync();
        }

        public async Task MarkSlotAsAvailableAsync(int slotId)
        {
            var slot = await _availabilitySlotRepository.GetByIdAsync(slotId);
            if (slot == null)
                throw new ArgumentException("Availability slot not found");

            slot.IsBooked = false;
            _availabilitySlotRepository.Update(slot);
            await _availabilitySlotRepository.SaveAsync();
        }
    }
}
