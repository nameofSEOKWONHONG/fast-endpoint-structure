using Infrastructure.EF;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Feature.Product.Plan.Entities;

public class PlanApprovalLine : EntityBase
{
    public long Id { get; set; }
    public long ProductPlanId { get; set; }
    public ProductPlan ProductPlan { get; set; }
    public string UserId { get; set; }
}

public class PlanApprovalLineBuilder : EntityBuilderBase<PlanApprovalLine>
{
    public override void Build(ModelBuilder builder)
    {
        builder.Entity<PlanApprovalLine>(e =>
        {
            e.ToTable($"{nameof(PlanApprovalLine)}s", "product");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id)
                .ValueGeneratedOnAdd();
            e.Property(x => x.UserId)
                .HasMaxLength(64);
            e.HasOne<ProductPlan>()
                .WithMany(x => x.ApprovalLines)
                .HasForeignKey(x => x.ProductPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        base.Build(builder);
    }
}