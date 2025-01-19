using Feature.Domain.Base;
using Feature.Domain.Weather.Result;

namespace Feature.Domain.Weather.Abstract;

public interface IGetWeatherService : IServiceImpl<int, Results<GetWeatherResult>>
{
}