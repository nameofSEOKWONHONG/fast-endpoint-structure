using Feature.Product.Plan.Entities;
using Microsoft.EntityFrameworkCore;

namespace Feature.Product.Core;

public class ProductDbContext : DbContext
{
    public ProductDbContext(DbContextOptions<ProductDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("product");
        // this.OnModelCreating(modelBuilder, new List<IEntityBuilderBase>()
        // {
        //     
        // });
    }
    
    public DbSet<ProductPlan> ProductPlans { get; set; }
    public DbSet<ApprovalLine> ApprovalLines { get; set; }
}