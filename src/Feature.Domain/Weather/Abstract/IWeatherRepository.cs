using Feature.Domain.Base;
using Feature.Domain.Weather.Request;

namespace Feature.Domain.Weather.Abstract;

public interface IWeatherRepository : IRepositoryBase
{
    Task<int> Insert(WeatherForecastDto dto, CancellationToken cancellationToken);
    Task<bool> Update(WeatherForecastDto dto, CancellationToken cancellationToken);
    Task<bool> Delete(int id, CancellationToken cancellationToken);
}