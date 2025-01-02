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
        this.OnModelCreating(modelBuilder, [
            new ProductPlanBuilder(),
            new ApprovalLineBuilder()
        ]);
    }
    
    public DbSet<ProductPlan> ProductPlans { get; set; }
    public DbSet<ApprovalLine> ApprovalLines { get; set; }
}