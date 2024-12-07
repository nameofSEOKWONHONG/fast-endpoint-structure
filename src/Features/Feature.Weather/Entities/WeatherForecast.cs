using Infrastructure.EF;
using Infrastructure.Entity;
using Microsoft.EntityFrameworkCore;

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

public class WeatherForecastBuilder : EntityBuilderBase<WeatherForecast>
{
    public override void Build(ModelBuilder builder)
    {
        builder.Entity<WeatherForecast>(e =>
        {
            e.ToTable(nameof(WeatherForecast), "example");
            e.HasKey(x => x.Id);
            e.Property(x => x.Id).ValueGeneratedOnAdd();
            e.Property(m => m.Summary).HasMaxLength(100);
        });
        base.Build(builder);
    }
}