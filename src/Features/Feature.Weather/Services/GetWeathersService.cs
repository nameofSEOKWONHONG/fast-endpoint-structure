using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Feature.Domain.Weather.Result;
using Feature.Weather.Core;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public class GetWeathersService : ServiceBase<GetWeathersService, WeatherDbContext>, IGetWeathersService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public GetWeathersService(ILogger<GetWeathersService> logger, ISessionContext sessionContext, WeatherDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public async Task<JPaginatedResult<GetWeatherResult>> HandleAsync(GetWeathersRequest request)
    {
        var total = await this.DbContext.WeatherForecasts.CountAsync();
        var result = await this.DbContext.WeatherForecasts
            .AsNoTracking()
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new GetWeatherResult(m.Id, m.Date, m.TemperatureC, m.Summary))
            .ToListAsync();
        
        return await JPaginatedResult<GetWeatherResult>.SuccessAsync(result, total, request.PageNo, request.PageSize);
    }
}