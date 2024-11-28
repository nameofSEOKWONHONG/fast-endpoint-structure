using Microsoft.AspNetCore.Identity;

namespace MovieSharpApi.Features.Auth.Entities;

public class RoleClaim : IdentityRoleClaim<string>
{
    public string Description { get; set; }
    public string Group { get; set; }
    public Role Role { get; set; }
}