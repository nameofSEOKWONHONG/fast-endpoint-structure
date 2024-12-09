using Feature.Domain.Weather.Request;
using Infrastructure.Domains;

namespace Feature.Domain.Weather.Abstract;

public interface ICreateWeatherService
{
    Task<JResults<bool>> HandleAsync(CreateWeatherForecastRequest request);
}
