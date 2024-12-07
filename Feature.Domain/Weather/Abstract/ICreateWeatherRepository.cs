using Feature.Domain.Weather.Request;

namespace Feature.Domain.Weather.Abstract;

public interface ICreateWeatherRepository
{
    Task<bool> HandleAsync(CreateWeatherForecastRequest request);    
}