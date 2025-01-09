using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Feature.Domain.Weather.Result;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public class GetWeathersService : ServiceBase<GetWeathersService, WeatherDbContext, GetWeathersRequest, JPaginatedResult<GetWeatherResult>>, IGetWeathersService
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

    public override async Task<JPaginatedResult<GetWeatherResult>> HandleAsync(GetWeathersRequest request, CancellationToken ct)
    {
        var total = await this.DbContext.WeatherForecasts.CountAsync(cancellationToken: ct);
        var result = await this.DbContext.WeatherForecasts
            .AsNoTracking()
            .Skip((request.PageNo - 1) * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new GetWeatherResult(m.Id, m.Date, m.TemperatureC, m.Summary))
            .ToListAsync(cancellationToken: ct);
        
        return await JPaginatedResult<GetWeatherResult>.SuccessAsync(result, total, request.PageNo, request.PageSize);
    }
}