using Infrastructure.EF;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Feature.Product.Plan.Entities;

public class ProductPlan : EntityBase
{
    public long Id { get; set; }
    public string Title { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string Description { get; set; }
    public double Price { get; set; }
    public string Step { get; set; }
    public virtual ICollection<PlanApprovalLine> ApprovalLines { get; set; }
}

public class ProductPlanBuilder : EntityBuilderBase<ProductPlan>
{
    public override void Build(ModelBuilder builder)
    {
        builder.Entity<ProductPlan>(e =>
        {
            e.ToTable($"{nameof(ProductPlan)}s", "product");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.HasMany<PlanApprovalLine>()
                .WithOne(x => x.ProductPlan)
                .HasForeignKey(x => x.ProductPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        base.Build(builder);
    }
}

