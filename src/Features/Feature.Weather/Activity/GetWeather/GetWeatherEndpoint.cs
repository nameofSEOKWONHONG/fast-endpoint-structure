using FastEndpoints;
using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Result;

namespace Feature.Weather.Activity.GetWeather;

public class GetWeatherEndpoint : EndpointWithoutRequest<JResults<GetWeatherResult>>
{
    private readonly IGetWeatherService _service;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="service"></param>
    public GetWeatherEndpoint(IGetWeatherService service)
    {
        _service = service;
    }
    
    public override void Configure()
    {
        Get("/api/weather/{id}");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        this.Response = await _service.HandleAsync(id, ct);
    }
}