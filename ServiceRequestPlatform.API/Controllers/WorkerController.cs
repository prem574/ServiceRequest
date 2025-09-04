using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceRequestPlatform.Application.DTOs.AvailabilitySlot;
using ServiceRequestPlatform.Application.DTOs.Rating;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;
using ServiceRequestPlatform.Application.DTOs.Worker;
using ServiceRequestPlatform.Application.Services.Interface;

namespace ServiceRequestPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkerService _workerService;

        public WorkersController(IWorkerService workerService)
        {
            _workerService = workerService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkerDto>> Register([FromBody] RegisterWorkerDto dto)
        {
            try
            {
                var worker = await _workerService.RegisterAsync(dto);
                return CreatedAtAction(nameof(GetProfile), new { id = worker.Id }, worker);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<WorkerDto>> Login([FromBody] WorkerLoginDto dto)
        {
            try
            {
                var worker = await _workerService.LoginAsync(dto);
                return Ok(worker);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{id}/profile")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<WorkerDto>> GetProfile(int id)
        {
            try
            {
                var worker = await _workerService.GetProfileAsync(id);
                return Ok(worker);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/profile")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult> UpdateProfile(int id, [FromBody] UpdateWorkerDto dto)
        {
            if (id != dto.WorkerId)
                return BadRequest("ID mismatch");

            try
            {
                await _workerService.UpdateProfileAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("{workerId}/requests")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetAssignedRequests(int workerId)
        {
            var requests = await _workerService.GetAssignedRequestsAsync(workerId);
            return Ok(requests);
        }

        [HttpGet("{workerId}/availability")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<IEnumerable<AvailabilitySlotDto>>> GetAvailability(int workerId)
        {
            var availability = await _workerService.GetAvailabilityAsync(workerId);
            return Ok(availability);
        }

        [HttpPost("availability")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<AvailabilitySlotDto>> AddAvailability([FromBody] CreateAvailabilitySlotDto dto)
        {
            try
            {
                var slot = await _workerService.AddAvailabilityAsync(dto);
                return CreatedAtAction(nameof(GetAvailability), new { workerId = dto.WorkerId }, slot);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("availability")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult> UpdateAvailability([FromBody] UpdateAvailabilitySlotDto dto)
        {
            try
            {
                await _workerService.UpdateAvailabilityAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("availability/{slotId}")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult> DeleteAvailability(int slotId)
        {
            try
            {
                await _workerService.DeleteAvailabilityAsync(slotId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("request-status")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult> UpdateRequestStatus([FromBody] UpdateServiceRequestStatusDto dto)
        {
            try
            {
                await _workerService.UpdateRequestStatusAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{workerId}/ratings")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetMyRatings(int workerId)
        {
            var ratings = await _workerService.GetMyRatingsAsync(workerId);
            return Ok(ratings);
        }

        [HttpGet("{workerId}/average-rating")]
        [Authorize(Roles = "Worker")]
        public async Task<ActionResult<double>> GetAverageRating(int workerId)
        {
            var average = await _workerService.GetAverageRatingAsync(workerId);
            return Ok(new { AverageRating = average });
        }
    }
}