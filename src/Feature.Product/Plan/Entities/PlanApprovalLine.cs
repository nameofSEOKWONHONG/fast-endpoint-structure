using Infrastructure.EF;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Product.Plan.Entities;

public class PlanApprovalLine : EntityBase
{
    public long Id { get; set; }
    public long ProductPlanId { get; set; }
    public ProductPlan ProductPlan { get; set; }
    public string UserId { get; set; }
}

internal class PlanApprovalLineConfiguration : IEntityTypeConfiguration<PlanApprovalLine>
{
    public void Configure(EntityTypeBuilder<PlanApprovalLine> builder)
    {
        builder.ToTable($"{nameof(PlanApprovalLine)}s", "product");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id)
           .ValueGeneratedOnAdd();
        builder.Property(x => x.UserId)
           .HasMaxLength(64);
        builder.Property(x => x.CreatedBy).HasMaxLength(36);
        builder.Property(x => x.ModifiedBy).HasMaxLength(36);        
        builder.HasOne<ProductPlan>()
            .WithMany(x => x.ApprovalLines)
            .HasForeignKey(x => x.ProductPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}