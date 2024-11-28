using Feature.Weather.Entities;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;
using MovieSharpApi.Infrastructure.EF;

namespace Feature.Weather.Database;

public class WeatherDbContext : DbContextBase<WeatherDbContext>
{
    public WeatherDbContext(DbContextOptions<WeatherDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        this.OnModelCreating(modelBuilder, new List<IEntityBuilderBase>()
        {
            new WeatherForecastBuilder()
        });
    }
    
    public DbSet<WeatherForecast> WeatherForecasts { get; set; }
}