﻿using System;


namespace ServiceRequestPlatform.Application.DTOs.Admin
{
    public class AdminLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
