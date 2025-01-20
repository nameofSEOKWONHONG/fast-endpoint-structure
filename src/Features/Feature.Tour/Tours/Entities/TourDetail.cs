using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Tour.Tours.Entities;

public class TourDetail : EntityBase
{
    public int Id { get; set; }
    public string TourId { get; set; }
    public Tour Tour { get; set; }
}

public class TourDetailConfiguration : IEntityTypeConfiguration<TourDetail>
{
    public void Configure(EntityTypeBuilder<TourDetail> builder)
    {
        builder.ToTable($"{nameof(TourDetail)}s", "tour");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.HasOne(x => x.Tour)
            .WithMany(x => x.TourDetails)
            .HasForeignKey(x => x.TourId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}