using FastEndpoints;
using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;

namespace Feature.Weather.Activity.CreateWeather;

public class CreateWeatherEndpoint : Endpoint<CreateWeatherForecastRequest, JResults<bool>>
{
    private readonly ICreateWeatherService _service;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="service"></param>
    public CreateWeatherEndpoint(ICreateWeatherService service)
    {
        _service = service;
    }
    
    public override void Configure()
    {
        Post("/api/weatherforecast");
    }

    public override async Task HandleAsync(CreateWeatherForecastRequest req, CancellationToken ct)
    {
        this.Response = await _service.HandleAsync(req);
    }
}