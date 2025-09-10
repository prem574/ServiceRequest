using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ServiceRequestPlatform.Application.DTOs.Auth;
using ServiceRequestPlatform.Application.Services.Implementations;
using ServiceRequestPlatform.Application.Services.Interface;
using ServiceRequestPlatform.Domain.Entities;
using ServiceRequestPlatform.Infrastructure.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ServiceRequestPlatform.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        private readonly IPasswordService _passwordService;


        public AuthController(AppDbContext context, IConfiguration config, IPasswordService passwordService)
        {
            _context = context;
            _config = config;
            _passwordService = passwordService;
        }


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginDto dto)
        {
            //  Customer
            var customer = await _context.Customers.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (customer != null && _passwordService.VerifyPassword(dto.Password, customer.PasswordHash))
            {
                var token = GenerateJwtToken(customer.Email, "Customer");
                return Ok(new { Token = token, Role = "Customer", UserId = customer.Id });
            }


            //  Worker
            var worker = await _context.Workers.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (worker != null && _passwordService.VerifyPassword(dto.Password, worker.PasswordHash))
            {
                var token = GenerateJwtToken(worker.Email, "Worker");
                return Ok(new { Token = token, Role = "Worker", UserId = worker.Id });
            }


            //  Admin
            var admin = await _context.Admins.FirstOrDefaultAsync(u => u.Email == dto.Email);
            if (admin != null && _passwordService.VerifyPassword(dto.Password, admin.PasswordHash))
            {
                var token = GenerateJwtToken(admin.Email, "Admin");
                return Ok(new { Token = token, Role = "Admin", UserId = admin.Id });
            }


            return Unauthorized("Invalid credentials");
        }


        private string GenerateJwtToken(string email, string role)
        {
            var claims = new[]
            {
new Claim(ClaimTypes.Email, email),
new Claim(ClaimTypes.Role, role)
};


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.Now.AddHours(2),
            signingCredentials: creds);


            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}