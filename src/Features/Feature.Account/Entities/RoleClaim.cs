using Microsoft.AspNetCore.Identity;

namespace Feature.Account.Entities;

public class RoleClaim : IdentityRoleClaim<string>
{
    public string Description { get; set; }
    public string Group { get; set; }
    public Role Role { get; set; }
}