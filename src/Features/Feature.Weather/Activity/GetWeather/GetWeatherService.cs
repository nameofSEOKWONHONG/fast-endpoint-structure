using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Result;
using Feature.Weather.Core;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Activity.GetWeather;

public class GetWeatherService : ServiceBase<GetWeatherService, WeatherDbContext>, IGetWeatherService
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


    public async Task<JResults<GetWeatherResult>> HandleAsync(int id, CancellationToken ct)
    {
        var item =await this.DbContext.WeatherForecasts.AsNoTracking()
            .Where(w => w.Id == id)
            .Select(m => new GetWeatherResult(m.Id, m.Date, m.TemperatureC, m.Summary))
            .FirstAsync(cancellationToken: ct);

        return await JResults<GetWeatherResult>.SuccessAsync(item);
    }
}