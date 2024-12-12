using Feature.Domain.Base;
using Feature.Domain.Weather.Request;

namespace Feature.Domain.Weather.Abstract;

public interface ICreateWeatherService
{
    Task<JResults<bool>> HandleAsync(CreateWeatherForecastRequest request);
}
