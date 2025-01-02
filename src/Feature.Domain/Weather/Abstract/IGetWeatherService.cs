using Feature.Domain.Base;
using Feature.Domain.Weather.Result;

namespace Feature.Domain.Weather.Abstract;

public interface IGetWeatherService
{
    Task<JResults<GetWeatherResult>> HandleAsync(int id, CancellationToken ct = default);
}