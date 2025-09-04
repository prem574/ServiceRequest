using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceRequestPlatform.Application.DTOs.Admin;
using ServiceRequestPlatform.Application.DTOs.Customer;
using ServiceRequestPlatform.Application.DTOs.Service;
using ServiceRequestPlatform.Application.DTOs.ServiceRequest;
using ServiceRequestPlatform.Application.DTOs.Worker;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Domain.Entities;

namespace ServiceRequestPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AdminController : ControllerBase
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpPost("login")]
        [AllowAnonymous] // Allow anonymous access for login
        public async Task<ActionResult<AdminDto>> Login([FromBody] AdminLoginDto dto)
        {
            try
            {
                var admin = await _adminService.LoginAsync(dto);
                return Ok(admin);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }

        [HttpGet("customers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAllCustomers()
        {
            var customers = await _adminService.GetAllCustomersAsync();
            return Ok(customers);
        }

        [HttpGet("workers")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<WorkerDto>>> GetAllWorkers()
        {
            var workers = await _adminService.GetAllWorkersAsync();
            return Ok(workers);
        }

        [HttpGet("service-requests")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ServiceRequestDto>>> GetAllServiceRequests()
        {
            var requests = await _adminService.GetAllServiceRequestsAsync();
            return Ok(requests);
        }

        [HttpGet("services")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<ServiceDto>>> GetAllServices()
        {
            var services = await _adminService.GetAllServicesAsync();
            return Ok(services);
        }

        [HttpGet("services/{serviceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceDto>> GetServiceById(int serviceId)
        {
            var service = await _adminService.GetServiceByIdAsync(serviceId);
            return Ok(service);
        }

        [HttpPost("services")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceDto>> AddService([FromBody] CreateServiceDto dto)
        {
            try
            {
                var service = await _adminService.AddServiceAsync(dto);
                return CreatedAtAction(nameof(GetServiceById), new { serviceId = service.Id }, service);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("services")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateService([FromBody] UpdateServiceDto dto)
        {
            try
            {
                await _adminService.UpdateServiceAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpDelete("services/{serviceId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteService(int serviceId)
        {
            try
            {
                await _adminService.DeleteServiceAsync(serviceId);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("assign-worker")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AssignWorker([FromBody] AssignWorkerDto dto)
        {
            try
            {
                await _adminService.AssignWorkerAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("customers/{customerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<CustomerDto>> GetCustomerById(int customerId)
        {
            try
            {
                var customer = await _adminService.GetCustomerByIdAsync(customerId);
                return Ok(customer);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("workers/{workerId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<WorkerDto>> GetWorkerById(int workerId)
        {
            try
            {
                var worker = await _adminService.GetWorkerByIdAsync(workerId);
                return Ok(worker);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpGet("service-requests/{requestId}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<ServiceRequestDto>> GetServiceRequestById(int requestId)
        {
            try
            {
                var request = await _adminService.GetServiceRequestByIdAsync(requestId);
                return Ok(request);
            }
            catch (ArgumentException ex)
            {
                return NotFound(ex.Message);
            }
        }

        [HttpPost("change-password")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> ChangePassword([FromBody] ResetPasswordDto dto)
        {
            try
            {
                await _adminService.ChangePasswordAsync(dto);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(ex.Message);
            }
        }
    }
}