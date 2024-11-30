using FastEndpoints;
using Feature.Weather.Domains;
using Feature.Weather.Services;
using Infrastructure.Domains;

namespace Feature.Weather;

public class GetWeathersEndpoint : EndpointWithoutRequest<JResults<IEnumerable<WeatherForecastResponse>>>
{
    private readonly IWeatherService _weatherService;
    public GetWeathersEndpoint(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    public override void Configure()
    {
        Get("/api/weatherforecast");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result =await _weatherService.GetWeatherForecasts();
        this.Response = await JResults<IEnumerable<WeatherForecastResponse>>.SuccessAsync(result);
    }
}