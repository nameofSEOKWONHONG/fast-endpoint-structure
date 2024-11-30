using Feature.Account.Entities;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Feature.Account.Database;

public class AppDbContext : DbContextBase<AppDbContext>
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="options"></param>
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.ToTable(nameof(User));
            e.HasKey(x => x.Id);
            e.Property(x => x.Email).IsRequired();
            e.Property(x => x.Id).ValueGeneratedOnAdd();
        });
        
        modelBuilder.Entity<UserRole>(e =>
        {
            e.ToTable(nameof(UserRole));
            e.HasKey(x => new {x.UserId, x.RoleId });
        });
        modelBuilder.Entity<Role>(e =>
        {
            e.ToTable(nameof(Role));
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
        });
        
        modelBuilder.Entity<RoleClaim>(e =>
        {
            e.ToTable(nameof(RoleClaim));
            e.HasKey(x => new {x.Id, x.RoleId, x.ClaimType });
            e.Property(x => x.Id).ValueGeneratedOnAdd();            
        });
    }
    
    public DbSet<User> Users { get; set; }
    public DbSet<UserRole> UserRoles { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<RoleClaim> RoleClaims { get; set; }
}