﻿using System;

namespace ServiceRequestPlatform.Application.DTOs.Admin
{
    public class ChangePasswordDto
    {
        public int AdminId { get; set; }
        public string CurrentPassword { get; set; } = string.Empty;
        public string NewPassword { get; set; } = string.Empty;
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}
