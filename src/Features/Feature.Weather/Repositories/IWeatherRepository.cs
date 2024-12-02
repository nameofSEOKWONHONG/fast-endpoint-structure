using Feature.Weather.Entities;

namespace Feature.Weather.Repositories;

public interface IWeatherRepository
{
    Task Initialize();
    Task<WeatherForecast> Get(int id);
    Task Insert(WeatherForecast item);
    Task Update(WeatherForecast item);
    Task Delete(WeatherForecast item);
}