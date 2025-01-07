using Feature.Product.Plan.Entities;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Feature.Product.Core;

public class ProductDbContext : DbContextBase<ProductDbContext>
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="options"></param>
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("product");
        modelBuilder.ApplyConfiguration(new ProductPlanConfiguration());
        modelBuilder.ApplyConfiguration(new PlanApprovalLineConfiguration());
    }
    
    public DbSet<ProductPlan> ProductPlans { get; set; }
    public DbSet<PlanApprovalLine> ApprovalLines { get; set; }
}