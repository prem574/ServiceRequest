using ServiceRequestPlatform.Domain.Entities;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;

using System.Security.Claims;
using System.Text;


namespace ServiceRequestPlatform.Application.Services.Implementations
{
    public class AuthService
    {
        private readonly IConfiguration _config;
        public AuthService(IConfiguration config) => _config = config;


        public string GenerateJwtToken(int userId, string email, string role, string fullName)
        {
            var claims = new[]
            {
new Claim(JwtRegisteredClaimNames.Sub, userId.ToString()),
new Claim(JwtRegisteredClaimNames.Email, email),
new Claim(ClaimTypes.Role, role),
new Claim("FullName", fullName)
};


            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);


            var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:ExpireMinutes"])),
            signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
