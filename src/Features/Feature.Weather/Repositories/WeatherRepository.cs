using eXtensionSharp;
using Feature.Weather.Database;
using Feature.Weather.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Repositories;

public class WeatherRepository : RepositoryBase<WeatherRepository, WeatherDbContext>, IWeatherRepository
{
    string[] summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };
    
    public WeatherRepository(ILogger<WeatherRepository> logger, ISessionContext sessionContext, WeatherDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public async Task Initialize()
    {
        var item = await this.DbContext.WeatherForecasts.FirstOrDefaultAsync();
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
            
            await this.DbContext.WeatherForecasts.AddRangeAsync(items);
            await this.DbContext.SaveChangesAsync();
        }
    }

    public async Task<WeatherForecast> Get(int id)
    {
        return await this.DbContext.WeatherForecasts
            .Where(m => m.Id == id)
            .FirstOrDefaultAsync();
    }
    public async Task GetPaging(int pageNo, int pageSize) => await this.DbContext.WeatherForecasts.Skip((pageNo - 1) * pageSize).Take(pageSize).ToListAsync();

    public async Task Insert(WeatherForecast item)
    {
        var exists = await this.DbContext.WeatherForecasts.AnyAsync(m => m.Id == item.Id);
        if(exists.xIsNotEmpty()) { throw new Exception($"WeatherForecast with id: {item.Id} already exists."); }
        
        await this.DbContext.WeatherForecasts.AddAsync(item);
        await this.DbContext.SaveChangesAsync();
    }

    public async Task Update(WeatherForecast item)
    {
        var exists = await this.DbContext.WeatherForecasts.FirstOrDefaultAsync(m => m.Id == item.Id);
        if(exists.xIsEmpty()) { throw new Exception($"WeatherForecast with id: {item.Id} does not exist."); }
        
        exists.Summary = item.Summary;
        exists.TemperatureC = item.TemperatureC;
        
        this.DbContext.WeatherForecasts.Update(exists);
        await this.DbContext.SaveChangesAsync();
    }

    public async Task Delete(WeatherForecast item)
    {
        var exists = await this.DbContext.WeatherForecasts.FirstOrDefaultAsync(m => m.Id == item.Id);
        if(exists.xIsEmpty()) { throw new Exception($"WeatherForecast with id: {item.Id} does not exist."); }
        
        this.DbContext.WeatherForecasts.Remove(exists);
        await this.DbContext.SaveChangesAsync();
    }
}