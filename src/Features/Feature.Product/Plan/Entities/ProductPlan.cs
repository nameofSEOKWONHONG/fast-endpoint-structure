using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

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

internal class ProductPlanConfiguration : IEntityTypeConfiguration<ProductPlan>
{
    public void Configure(EntityTypeBuilder<ProductPlan> builder)
    {
        builder.ToTable($"{nameof(ProductPlan)}s", "product");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id);
        builder.Property(x => x.Title).HasMaxLength(200);
        builder.Property(x => x.Description).HasMaxLength(2000);
        builder.Property(x => x.CreatedBy).HasMaxLength(36);
        builder.Property(x => x.ModifiedBy).HasMaxLength(36);
        builder.HasMany<PlanApprovalLine>()
            .WithOne(x => x.ProductPlan)
            .HasForeignKey(x => x.ProductPlanId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}

