using Feature.Weather.Domains;

namespace Feature.Weather.Services;

public interface IWeatherService
{
    Task<WeatherForecastResponse> GetWeatherForecast(int id);
    Task<IEnumerable<WeatherForecastResponse>> GetWeatherForecasts();
    Task<bool> SaveWeatherForecast(WeatherForecastRequest request);
    Task<bool> DeleteWeatherForecast(int id);
}