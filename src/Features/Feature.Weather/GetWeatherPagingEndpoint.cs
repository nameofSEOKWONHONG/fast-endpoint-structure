using FastEndpoints;
using Feature.Weather.Domains;
using Feature.Weather.Services;
using Infrastructure.Domains;

namespace Feature.Weather;

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
        Roles("Admin");
    }

    public override async Task HandleAsync(PagingRequest req, CancellationToken ct)
    {
        var items = await _weatherService.GetWeatherForecasts();
        var result  = items.Skip((req.PageNo - 1) * req.PageSize).Take(req.PageSize).ToList();
        this.Response = await JResults<IEnumerable<WeatherForecastResponse>>.SuccessAsync(result);
    }
}