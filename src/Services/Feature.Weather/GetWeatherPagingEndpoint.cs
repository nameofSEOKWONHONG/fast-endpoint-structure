using FastEndpoints;
using Infrastructure.Domains;
using MovieSharpApi.Features.Weather.Domains;
using MovieSharpApi.Features.Weather.Services;

namespace MovieSharpApi.Features.Weather;

public class PagingRequest
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}

public class GetWeatherPagingEndpoint : Endpoint<PagingRequest, JResults<IEnumerable<WeatherForecastResponse>>>
{
    private readonly IWeatherService _weatherService;
    public GetWeatherPagingEndpoint(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }

    /// <summary>
    /// APP 시작시 한번만 수행됨.
    /// </summary>
    public override void Configure()
    {
        Get("/api/weatherforecast/{pageno}/{pageSize}");
        Roles("Manager");
    }

    public override async Task HandleAsync(PagingRequest req, CancellationToken ct)
    {
        var items = await _weatherService.GetWeatherForecasts();
        var result  = items.Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToList();
        this.Response = await JResults<IEnumerable<WeatherForecastResponse>>.SuccessAsync(result);
    }
}