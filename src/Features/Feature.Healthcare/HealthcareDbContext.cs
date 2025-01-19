using Infrastructure.EF;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Healthcare;

public class HealthcareDbContext : DbContextBase<HealthcareDbContext>
{
    public HealthcareDbContext(DbContextOptions<HealthcareDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MetadataConfiguration());
    }
    
    public DbSet<Metadata> Metadatas { get; set; }
}

public class Metadata : EntityBase
{
    public string Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int HeartRate { get; set; }
    public int Bpm { get; set; }
}

public class MetadataConfiguration : IEntityTypeConfiguration<Metadata>
{
    public void Configure(EntityTypeBuilder<Metadata> builder)
    {
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).IsRequired();
        builder.Property(x => x.StartDate).IsRequired();
        builder.Property(x => x.HeartRate).IsRequired();
        builder.Property(x => x.Bpm).IsRequired();
    }
}