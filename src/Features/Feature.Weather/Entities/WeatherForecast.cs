using Infrastructure.EF;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Feature.Weather.Entities;

public class WeatherForecast : EntityBase
{
    public int Id { get; set; }
    public DateOnly Date { get; set; }
    public int TemperatureC { get; set; }
    public string Summary { get; set; }

    public WeatherForecast()
    {
        
    }

    public WeatherForecast(int id, DateOnly date, int temperatureC, string summary, string createdBy, DateTime createdOn)
    {
        Id = id;
        Date = date;
        TemperatureC = temperatureC;
        Summary = summary;
        CreatedBy = createdBy;
        CreatedOn = createdOn;
    }
}

internal class WeatherForecastConfiguration : IEntityTypeConfiguration<WeatherForecast>
{
    public void Configure(EntityTypeBuilder<WeatherForecast> builder)
    {
        builder.ToTable($"{nameof(WeatherForecast)}s", "example");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).ValueGeneratedOnAdd();
        builder.Property(m => m.Summary).HasMaxLength(100);
    }
}