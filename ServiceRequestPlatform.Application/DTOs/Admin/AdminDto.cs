using System;

namespace ServiceRequestPlatform.Application.DTOs.Admin
{
    public class AdminDto
    {
        public int Id { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
