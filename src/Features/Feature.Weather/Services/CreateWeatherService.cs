using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Feature.Weather.Core;
using Feature.Weather.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public class CreateWeatherService : ServiceBase<CreateWeatherService, WeatherDbContext>, ICreateWeatherService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public CreateWeatherService(ILogger<CreateWeatherService> logger, ISessionContext sessionContext, WeatherDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public async Task<JResults<bool>> HandleAsync(CreateWeatherForecastRequest request, CancellationToken cancellationToken)
    {
        var newItem = new WeatherForecast(0, request.Date, request.TemperatureC, request.Summary, this.SessionContext.User.UserName, this.SessionContext.Date.Now);
        await this.DbContext.WeatherForecasts.AddAsync(newItem, cancellationToken);
        await this.DbContext.SaveChangesAsync(cancellationToken);
        return await JResults<bool>.SuccessAsync(newItem.Id > 0);
    }
}