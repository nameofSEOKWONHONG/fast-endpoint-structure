using Feature.Weather.Repositories;
using Feature.Weather.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Feature.Weather;

public static class AssemblyWeatherExtensions
{
    public static void AddWeatherFeature(this IServiceCollection services)
    {
        services.AddScoped<IWeatherRepository, WeatherRepository>();
        services.AddScoped<IWeatherService, WeatherService>();
    }
}