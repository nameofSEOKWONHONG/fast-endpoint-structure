using Feature.Domain.Weather.Result;
using Infrastructure.Domains;

namespace Feature.Domain.Weather.Abstract;

public interface IGetWeatherService
{
    Task<JResults<GetWeatherResult>> HandleAsync(int id);
}