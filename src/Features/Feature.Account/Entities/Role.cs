using Microsoft.AspNetCore.Identity;

namespace Feature.Account.Entities;

public class Role : IdentityRole
{
    public string Description { get; set; }
    
    public virtual ICollection<RoleClaim> RoleClaims { get; set; }

    public Role()
    {
        RoleClaims = new HashSet<RoleClaim>();
    }
}