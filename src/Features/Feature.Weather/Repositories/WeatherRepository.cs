using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Feature.Weather.Core;
using Feature.Weather.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
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

    public async Task<bool> Insert(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var newItem = new WeatherForecast(0, request.Date, request.TemperatureC, request.Summary, "TEST", DateTime.Now);
        await this.DbContext.WeatherForecasts.AddAsync(newItem, cancellationToken);
        await this.DbContext.SaveChangesAsync(cancellationToken);
        return newItem.Id > 0;
    }
}