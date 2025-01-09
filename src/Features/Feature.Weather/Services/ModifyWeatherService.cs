using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public interface IModifyWeatherService : IServiceImpl<WeatherForecastDto, JResults<bool>>
{
    
}

public class ModifyWeatherService : ServiceRepoBase<ModifyWeatherService, IWeatherRepository, WeatherForecastDto, JResults<bool>>, IModifyWeatherService
{
    public ModifyWeatherService(ILogger<ModifyWeatherService> logger, ISessionContext sessionContext, IWeatherRepository repository) : base(logger, sessionContext, repository)
    {
    }

    public override async Task<JResults<bool>> HandleAsync(WeatherForecastDto request, CancellationToken cancellationToken)
    {
        var result = await this.Repository.Update(request, cancellationToken);
        if (result) return await JResults<bool>.SuccessAsync();
        return await JResults<bool>.FailAsync();
    }
}