using eXtensionSharp;
using Feature.Weather.Core;
using Feature.Weather.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Activity.InitializeWeather;

public interface IInitializeWeatherService
{
    Task HandleAsync();
}

public class InitializeWeatherService: ServiceBase<InitializeWeatherService, WeatherDbContext>, IInitializeWeatherService
{
    string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    public InitializeWeatherService(ILogger<InitializeWeatherService> logger, ISessionContext sessionContext, WeatherDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public async Task HandleAsync()
    {
        var item = await this.DbContext.WeatherForecasts.FirstOrDefaultAsync();
        if (item.xIsEmpty())
        {
            var items = Enumerable.Range(1, 50).Select(index =>
                    new WeatherForecast
                    (
                        0,
                        DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
                        Random.Shared.Next(-20, 55),
                        summaries[Random.Shared.Next(summaries.Length)],
                        "TEST",
                        DateTime.Now
                    ))
                .ToList();
            
            await this.DbContext.WeatherForecasts.AddRangeAsync(items);
            await this.DbContext.SaveChangesAsync();
        }
    }
}