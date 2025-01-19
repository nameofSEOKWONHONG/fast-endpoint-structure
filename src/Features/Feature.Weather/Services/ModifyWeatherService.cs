using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public interface IModifyWeatherService : IServiceImpl<WeatherForecastDto, Results<bool>>
{
    
}

public class ModifyWeatherService : ServiceRepoBase<ModifyWeatherService, IWeatherRepository, WeatherForecastDto, Results<bool>>, IModifyWeatherService
{
    public ModifyWeatherService(ILogger<ModifyWeatherService> logger, ISessionContext sessionContext, IWeatherRepository repository) : base(logger, sessionContext, repository)
    {
    }

    public override async Task<Results<bool>> HandleAsync(WeatherForecastDto request, CancellationToken cancellationToken)
    {
        var result = await this.Repository.Update(request, cancellationToken);
        if (result) return await Results<bool>.SuccessAsync();
        return await Results<bool>.FailAsync();
    }
}