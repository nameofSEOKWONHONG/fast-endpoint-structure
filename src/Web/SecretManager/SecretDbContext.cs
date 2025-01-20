using Microsoft.EntityFrameworkCore;
using SecretManager.Entities;

namespace SecretManager;

public class SecretDbContext : DbContext
{
    public SecretDbContext(DbContextOptions<SecretDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new Secret.SecretConfiguration());
    }

    public void EnsureCreated()
    {
        Database.EnsureCreated();
    }

    public DbSet<Secret> Secrets { get; set; }
}