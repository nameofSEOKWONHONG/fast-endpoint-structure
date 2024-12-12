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
    public GetWeatherService(ILogger<GetWeatherService> logger, ISessionContext sessionContext, WeatherDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }


    public async Task<JResults<GetWeatherResult>> HandleAsync(int id)
    {
        var item =await this.DbContext.WeatherForecasts.AsNoTracking()
            .Where(w => w.Id == id)
            .Select(m => new GetWeatherResult(m.Id, m.Date, m.TemperatureC, m.Summary))
            .FirstAsync();

        return await JResults<GetWeatherResult>.SuccessAsync(item);
    }
}