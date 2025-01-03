using Feature.Domain.Base;
using Feature.Domain.Weather.Request;

namespace Feature.Domain.Weather.Abstract;

public interface IServiceImpl<in TRequest, TResult>
{
    Task<TResult> HandleAsync(TRequest request, CancellationToken ct);
}

public interface ICreateWeatherService : IServiceImpl<CreateWeatherForecastRequest, JResults<bool>>
{
    
}
