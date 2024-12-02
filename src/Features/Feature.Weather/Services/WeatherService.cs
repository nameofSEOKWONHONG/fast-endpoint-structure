using eXtensionSharp;
using Feature.Weather.Database;
using Feature.Weather.Domains;
using Feature.Weather.Entities;
using Infrastructure.Base;
using Infrastructure.KeyValueManager;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public class WeatherService : ServiceBase<WeatherService, WeatherDbContext>, IWeatherService
{
    private readonly HybridCache _cache;
    private readonly IKeyValueLoader _keyValueLoader;

    public WeatherService(ILogger<WeatherService> logger, ISessionContext sessionContext, WeatherDbContext dbContext,
        HybridCache cache, IKeyValueLoader loader) : base(logger, sessionContext, dbContext)
    {
        _cache = cache;
        _keyValueLoader = loader;
    }

    public async Task<WeatherForecastResponse> GetWeatherForecast(int id)
    {
        var item = await this.DbContext.WeatherForecasts
            .Where(m => m.Id == id)
            .Select(m => 
                new WeatherForecastResponse(m.Id, m.Date, m.TemperatureC, m.Summary))
            .FirstOrDefaultAsync();
        return item;
    }

    public async Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecasts()
    {
        var items = await this.DbContext.WeatherForecasts
            .Select(m => new WeatherForecastResponse(m.Id, m.Date, m.TemperatureC, m.Summary))
            .ToListAsync();
        return items;
    }

    public async Task<bool> SaveWeatherForecast(WeatherForecastRequest request)
    {
        var newItem = new WeatherForecast(0, request.Date, request.TemperatureC, request.Summary, "TEST", DateTime.Now);
        await this.DbContext.WeatherForecasts.AddAsync(newItem);
        await this.DbContext.SaveChangesAsync();
        return newItem.Id > 0;
    }

    public async Task<bool> DeleteWeatherForecast(int id)
    {
        var exists = await this.DbContext.WeatherForecasts.FirstOrDefaultAsync(m => m.Id == id);
        if(exists == null) return false;
        
        this.DbContext.WeatherForecasts.Remove(exists);
        await this.DbContext.SaveChangesAsync();

        return true;
    }
        
}