using FastEndpoints;
using Infrastructure.Domains;
using MovieSharpApi.Features.Weather.Domains;
using MovieSharpApi.Features.Weather.Services;

namespace MovieSharpApi.Features.Weather;

public class SaveWeatherEndpoint : Endpoint<WeatherForecastRequest, JResults<bool>>
{
    private readonly IWeatherService _weatherService;
    public SaveWeatherEndpoint(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }
    
    public override void Configure()
    {
        Post("/api/weatherforecast");
        Roles("Admin");
    }

    public override async Task HandleAsync(WeatherForecastRequest req, CancellationToken ct)
    {
        var result = await _weatherService.SaveWeatherForecast(req);
        this.Response = await JResults<bool>.SuccessAsync(result); 
    }
}