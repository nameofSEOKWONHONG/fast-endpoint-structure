using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Feature.Weather.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public class CreateWeatherService : ServiceRepoBase<CreateWeatherService, IWeatherRepository, WeatherForecastDto, JResults<int>>, ICreateWeatherService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public CreateWeatherService(ILogger<CreateWeatherService> logger, ISessionContext sessionContext, IWeatherRepository repository) : base(logger, sessionContext, repository)
    {
    }

    public override async Task<JResults<int>> HandleAsync(WeatherForecastDto dto, CancellationToken cancellationToken)
    {
        var result = await this.Repository.Insert(dto, cancellationToken);
        return await JResults<int>.SuccessAsync(result);
    }
}