using System;


namespace ServiceRequestPlatform.Application.DTOs.Admin
{
    public class ResetPasswordDto
    {
        public int AdminId { get; set; }
        public string Email { get; set; } = string.Empty;
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
