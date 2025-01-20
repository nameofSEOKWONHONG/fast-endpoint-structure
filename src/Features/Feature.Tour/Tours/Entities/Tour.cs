using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Tour.Tours.Entities;

public class Tour : EntityBase
{
    public string Id { get; set; }
    public TourType Type { get; set; }
    public virtual ICollection<TourDetail> TourDetails { get; set; }
}

public class TourConfiguration : IEntityTypeConfiguration<Tour>
{
    public void Configure(EntityTypeBuilder<Tour> builder)
    {
        builder.ToTable($"{nameof(Tour)}s", "tour");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasMany(x => x.TourDetails)
            .WithOne(x => x.Tour)
            .HasForeignKey(x => x.TourId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
