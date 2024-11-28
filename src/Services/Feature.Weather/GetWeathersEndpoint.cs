using FastEndpoints;
using Infrastructure.Domains;
using MovieSharpApi.Features.Weather.Domains;
using MovieSharpApi.Features.Weather.Services;

namespace MovieSharpApi.Features.Weather;

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
        Roles("Manager", "Auditor");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var result =await _weatherService.GetWeatherForecasts();
        this.Response = await JResults<IEnumerable<WeatherForecastResponse>>.SuccessAsync(result);
    }
}