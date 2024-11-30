using FastEndpoints;
using Feature.Weather.Domains;
using Feature.Weather.Services;
using Infrastructure.Domains;

namespace Feature.Weather;

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
    }

    public override async Task HandleAsync(WeatherForecastRequest req, CancellationToken ct)
    {
        var result = await _weatherService.SaveWeatherForecast(req);
        this.Response = await JResults<bool>.SuccessAsync(result); 
    }
}