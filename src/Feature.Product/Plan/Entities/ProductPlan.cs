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
    public virtual ICollection<ApprovalLine> ApprovalLines { get; set; }
}

public class ProductPlanBuilder : EntityBuilderBase<ProductPlan>
{
    public override void Build(ModelBuilder builder)
    {
        builder.Entity<ProductPlan>(e =>
        {
            e.ToTable("plans", "product");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id);
            e.Property(x => x.Title).HasMaxLength(200);
            e.Property(x => x.Description).HasMaxLength(2000);
            e.HasMany<ApprovalLine>()
                .WithOne(x => x.ProductPlan)
                .HasForeignKey(x => x.ProductPlanId)
                .OnDelete(DeleteBehavior.Restrict);
        });
        base.Build(builder);
    }
}

public class ApprovalLine : EntityBase
{
    public long Id { get; set; }
    public long ProductPlanId { get; set; }
    public ProductPlan ProductPlan { get; set; }
    public string UserId { get; set; }
}

public class ApprovalLineBuilder : EntityBuilderBase<ApprovalLine>
{
    public override void Build(ModelBuilder builder)
    {
        builder.Entity<ApprovalLine>(e =>
        {
            e.ToTable("approval_lines", "product");
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