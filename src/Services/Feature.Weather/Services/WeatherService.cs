using Feature.Weather.Database;
using Feature.Weather.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Hybrid;
using MovieSharpApi.Features.Weather.Domains;

namespace MovieSharpApi.Features.Weather.Services;

public interface IWeatherService
{
    Task Initialize();
    Task<WeatherForecastResponse> GetWeatherForecast(int id);
    Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecasts();
    Task<bool> SaveWeatherForecast(WeatherForecastRequest request);
    Task<bool> DeleteWeatherForecast(int id);
}

public class WeatherService : IWeatherService
{
    string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    private readonly HybridCache _cache;
    private readonly WeatherDbContext _context;
    public WeatherService(WeatherDbContext dbContext, HybridCache cache)
    {
        _context = dbContext;
        
        _cache = cache;
    }

    public async Task Initialize()
    {
        var item = await _context.WeatherForecasts.FirstOrDefaultAsync();
        if (item == null)
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
            
            await _context.WeatherForecasts.AddRangeAsync(items);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<WeatherForecastResponse> GetWeatherForecast(int id)
    {
        var item = await _context.WeatherForecasts
            .Where(m => m.Id == id)
            .Select(m => 
                new WeatherForecastResponse(m.Id, m.Date, m.TemperatureC, m.Summary))
            .FirstOrDefaultAsync();
        return item;
    }

    public async Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecasts()
    {
        var items = await _context.WeatherForecasts
            .Select(m => new WeatherForecastResponse(m.Id, m.Date, m.TemperatureC, m.Summary))
            .ToListAsync();
        return items;
    }

    public async Task<bool> SaveWeatherForecast(WeatherForecastRequest request)
    {
        var newItem = new WeatherForecast(0, request.Date, request.TemperatureC, request.Summary, "TEST", DateTime.Now);
        await _context.WeatherForecasts.AddAsync(newItem);
        await _context.SaveChangesAsync();
        return newItem.Id > 0;
    }

    public async Task<bool> DeleteWeatherForecast(int id)
    {
        var exists = await _context.WeatherForecasts.FirstOrDefaultAsync(m => m.Id == id);
        if(exists == null) return false;
        
        _context.WeatherForecasts.Remove(exists);
        await _context.SaveChangesAsync();

        return true;
    }
        
}