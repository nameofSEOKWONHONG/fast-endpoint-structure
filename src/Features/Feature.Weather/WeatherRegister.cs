using eXtensionSharp;
using Feature.Domain.Weather.Abstract;
using Feature.Weather.Activity.CreateWeather;
using Feature.Weather.Activity.GetWeather;
using Feature.Weather.Activity.GetWeathers;
using Feature.Weather.Activity.InitializeWeather;
using Feature.Weather.Core;
using Infrastructure.KeyValueManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Feature.Weather;

public static class WeatherRegister
{
    public static void AddWeatherFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICreateWeatherRepository, CreateWeatherRepository>();
        builder.Services.AddScoped<ICreateWeatherService, CreateWeatherService>();
        builder.Services.AddScoped<IGetWeatherService, GetWeatherService>();
        builder.Services.AddScoped<IGetWeathersService, GetWeathersService>();
        builder.Services.AddScoped<IInitializeWeatherService, InitializeWeatherService>();
        
        builder.Services
            .AddDbContext<WeatherDbContext>((s, options) =>
            {
                //var loader = s.GetRequiredService<IKeyValueLoader>();
                //var con = loader.Data["SQL_CONNECTION"].xValue<string>();
                
                var con = Environment.GetEnvironmentVariable("SQL_CONNECTION");
                options.UseSqlServer(con);
                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging()
                        .EnableThreadSafetyChecks()
                        .EnableDetailedErrors()
                        ;
                }
            });
    }

    public static async Task UseWeatherFeature(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        var service = scope.ServiceProvider.GetRequiredService<IInitializeWeatherService>();
        await service.HandleAsync();
    }
}