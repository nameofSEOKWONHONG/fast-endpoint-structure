﻿using eXtensionSharp;
using Feature.Weather.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public interface IInitializeWeatherService
{
    Task HandleAsync();
}

public class InitializeWeatherService: ServiceBase<InitializeWeatherService>, IInitializeWeatherService
{
    string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    private readonly WeatherDbContext _weatherDbContext;
    public InitializeWeatherService(ILogger<InitializeWeatherService> logger, ISessionContext sessionContext, WeatherDbContext dbContext) : base(logger, sessionContext)
    {
        _weatherDbContext = dbContext;
    }

    public async Task HandleAsync()
    {
        var item = await this._weatherDbContext.WeatherForecasts.FirstOrDefaultAsync();
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
            
            await this._weatherDbContext.WeatherForecasts.AddRangeAsync(items);
            await this._weatherDbContext.SaveChangesAsync();
        }
    }
}