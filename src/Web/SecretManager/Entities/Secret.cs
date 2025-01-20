using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SecretManager.Entities;

public class Secret
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Json { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }

    public Secret()
    {
        
    }

    public Secret(string title, string description, string json, DateTime createdAt, DateTime? updatedAt)
    {
        Title = title;
        Description = description;
        Json = json;
        CreatedAt = createdAt;
        UpdatedAt = updatedAt;
    }
    
    internal class SecretConfiguration : IEntityTypeConfiguration<Secret>
    {
        public void Configure(EntityTypeBuilder<Secret> builder)
        {
            builder.ToTable("Secrets", "dbo");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).ValueGeneratedOnAdd();
            builder.Property(x => x.Title).HasMaxLength(500);
            builder.Property(x => x.Description).HasMaxLength(2000);
            builder.Property(x => x.Json).HasMaxLength(8000);
        }
    }
}

