﻿using System;


namespace ServiceRequestPlatform.Application.DTOs.Auth
{
    public class AuthLoginDto
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
}
