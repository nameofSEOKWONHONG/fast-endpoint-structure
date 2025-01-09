using eXtensionSharp;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Feature.Weather.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Repositories;

public class WeatherRepository : RepositoryBase<WeatherRepository, WeatherDbContext>, IWeatherRepository
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public WeatherRepository(ILogger<WeatherRepository> logger, ISessionContext sessionContext, WeatherDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }
    
    public async Task<int> Insert(WeatherForecastDto dto, CancellationToken cancellationToken)
    {
        var exists = await this.DbContext.WeatherForecasts.FirstOrDefaultAsync(m => m.Id == dto.Id, cancellationToken);
        if (exists.xIsNotEmpty()) return 0;
        
        var newItem = new WeatherForecast(0, dto.Date, dto.TemperatureC, dto.Summary, "TEST", DateTime.Now);
        await this.DbContext.WeatherForecasts.AddAsync(newItem, cancellationToken);
        await this.DbContext.SaveChangesAsync(cancellationToken);
        return newItem.Id;
    }

    public async Task<bool> Update(WeatherForecastDto dto, CancellationToken cancellationToken)
    {
        var exists = await this.DbContext.WeatherForecasts.FirstOrDefaultAsync(m => m.Id == dto.Id, cancellationToken);
        if (exists.xIsEmpty()) return false;
        
        exists.Date = dto.Date;
        exists.TemperatureC = dto.TemperatureC;
        exists.Summary = dto.Summary;
        
        this.DbContext.WeatherForecasts.Update(exists);
        var states = await this.DbContext.SaveChangesAsync(cancellationToken);
        
        return states > 0;
    }

    public async Task<bool> Delete(int id, CancellationToken cancellationToken)
    {
        var exists = await this.DbContext.WeatherForecasts.FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        if (exists.xIsEmpty()) return false;
        this.DbContext.WeatherForecasts.Remove(exists);
        var states = await this.DbContext.SaveChangesAsync(cancellationToken);
        return states > 0;
    }
}