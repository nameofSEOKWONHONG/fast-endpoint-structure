using Feature.Domain.Weather.Request;

namespace Feature.Domain.Weather.Abstract;

public interface IWeatherRepository
{
    Task<bool> Insert(CreateWeatherForecastRequest request, CancellationToken cancellationToken);
}