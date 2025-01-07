using Feature.Weather.Entities;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Feature.Weather.Core;

public class WeatherDbContext : DbContextBase<WeatherDbContext>
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasDefaultSchema("example");
        modelBuilder.ApplyConfiguration(new WeatherForecastConfiguration());
    }
    
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}