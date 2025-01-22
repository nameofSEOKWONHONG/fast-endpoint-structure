using BlazorSecretManager.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager;

public class SecretDbContext : IdentityDbContext
{
    public SecretDbContext(DbContextOptions<SecretDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.ApplyConfiguration(new Secret.SecretConfiguration());
        modelBuilder.ApplyConfiguration(new UserConfiguration());
        modelBuilder.ApplyConfiguration(new MenuConfiguration());
    }

    public void EnsureCreated()
    {
        Database.EnsureCreated();
    }

    public DbSet<Secret> Secrets { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Menu> Menus { get; set; }
}



