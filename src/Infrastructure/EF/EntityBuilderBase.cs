using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public abstract class EntityBuilderBase<T> : IEntityBuilderBase
    where T : EntityBase
{
    public virtual void Build(ModelBuilder builder)
    {
        builder.Entity<T>(e =>
        {
            e.Property(m => m.CreatedBy)
                .HasMaxLength(36)
                .IsRequired()
                ;
            e.Property(m => m.ModifiedBy)
                .HasMaxLength(36)
                ;
            e.Property(m => m.IsActive)
                .HasDefaultValue(true);
        });
    }
}