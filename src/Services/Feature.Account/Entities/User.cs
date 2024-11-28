﻿using Microsoft.AspNetCore.Identity;

namespace MovieSharpApi.Features.Auth.Entities;

public class User : IdentityUser<string>
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
