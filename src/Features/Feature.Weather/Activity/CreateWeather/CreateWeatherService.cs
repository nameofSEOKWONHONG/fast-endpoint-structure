using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Activity.CreateWeather;


public class CreateWeatherService : ServiceBase<CreateWeatherService>, ICreateWeatherService
{
    private readonly ICreateWeatherRepository _repository;

    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="repository"></param>
    public CreateWeatherService(ILogger<CreateWeatherService> logger, ISessionContext sessionContext, ICreateWeatherRepository repository) : base(logger, sessionContext)
    {
        _repository = repository;
    }

    public async Task<JResults<bool>> HandleAsync(CreateWeatherForecastRequest request)
    {
        var result = await _repository.HandleAsync(request);
        return await JResults<bool>.SuccessAsync(result);
    }
}