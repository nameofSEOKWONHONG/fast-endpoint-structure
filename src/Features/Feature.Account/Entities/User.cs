using Microsoft.AspNetCore.Identity;

namespace Feature.Account.Entities;

public class User : IdentityUser<string>
{
    public string Token { get; set; }
    public string RefreshToken { get; set; }
}
