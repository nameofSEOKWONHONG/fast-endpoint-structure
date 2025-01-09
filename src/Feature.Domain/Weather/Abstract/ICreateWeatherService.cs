using Feature.Domain.Base;
using Feature.Domain.Weather.Request;

namespace Feature.Domain.Weather.Abstract;


public interface ICreateWeatherService : IServiceImpl<WeatherForecastDto, JResults<int>>
{
    
}
