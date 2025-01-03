using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Result;
using Feature.Weather.Core;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public class GetWeatherService : ServiceBase<GetWeatherService, WeatherDbContext, int, JResults<GetWeatherResult>>, IGetWeatherService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public GetWeatherService(ILogger<GetWeatherService> logger, ISessionContext sessionContext, WeatherDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public override async Task<JResults<GetWeatherResult>> HandleAsync(int request, CancellationToken cancellationToken)
    {
        var item = await this.DbContext.WeatherForecasts.AsNoTracking()
            .Where(m => m.Id == request)
            .Select(m => new GetWeatherResult(m.Id, m.Date, m.TemperatureC, m.Summary))
            .FirstAsync(cancellationToken: cancellationToken);

        return await JResults<GetWeatherResult>.SuccessAsync(item);
    }
}