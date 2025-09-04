using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceRequestPlatform.Application.DTOs.Customer;
using ServiceRequestPlatform.Application.DTOs.Rating;
using ServiceRequestPlatform.Application.DTOs.Service;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;
using ServiceRequestPlatform.Application.Services.Interface;

namespace ServiceRequestPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerDto>> Register([FromBody] CustomerRegisterDto dto)
        {
            try
            {
                var customer = await _customerService.RegisterAsync(dto);
                return CreatedAtAction(nameof(GetProfile), new { id = customer.Id }, customer);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<CustomerDto>> Login([FromBody] CustomerLoginDto dto)
        {
            try
            {
                var customer = await _customerService.LoginAsync(dto);
                return Ok(customer);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("{id}/profile")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<CustomerDto>> GetProfile(int id)
        {
            try
            {
                var customer = await _customerService.GetProfileAsync(id);
                return Ok(customer);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPut("{id}/profile")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> UpdateProfile(int id, [FromBody] CustomerDto dto)
        {
            if (id != dto.Id)
                return BadRequest("ID mismatch");

            try
            {
                await _customerService.UpdateProfileAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("services")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetServices()
        {
            var services = await _customerService.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpPost("book-service")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ServiceRequestDto>> BookService([FromBody] BookServiceRequestDto dto)
        {
            try
            {
                var serviceRequest = await _customerService.BookServiceAsync(dto);
                return CreatedAtAction(nameof(GetMyRequests), new { customerId = dto.CustomerId }, serviceRequest);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("{customerId}/requests")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetMyRequests(int customerId)
        {
            var requests = await _customerService.GetMyRequestsAsync(customerId);
            return Ok(requests);
        }

        [HttpPut("reschedule-service")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<ServiceRequestDto>> RescheduleService([FromBody] RescheduleServiceRequestDto dto)
        {
            try
            {
                var serviceRequest = await _customerService.RescheduleServiceAsync(dto);
                return Ok(serviceRequest);
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

        [HttpDelete("cancel-service/{requestId}")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult> CancelService(int requestId)
        {
            try
            {
                await _customerService.CancelServiceAsync(requestId);
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

        [HttpPost("rate-service")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<RatingDto>> RateService([FromBody] CreateRatingDto dto)
        {
            try
            {
                var rating = await _customerService.RateServiceAsync(dto);
                return CreatedAtAction(nameof(GetMyRatings), new { customerId = dto.CustomerId }, rating);
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

        [HttpGet("{customerId}/ratings")]
        [Authorize(Roles = "Customer")]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetMyRatings(int customerId)
        {
            var ratings = await _customerService.GetMyRatingsAsync(customerId);
            return Ok(ratings);
        }
    }
}