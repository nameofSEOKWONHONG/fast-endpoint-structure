using FastEndpoints;
using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Feature.Domain.Weather.Result;

namespace Feature.Weather.Activity.GetWeathers;

public class GetWeathersEndpoint : Endpoint<GetWeathersRequest, JPaginatedResult<GetWeatherResult>>
{
    private readonly IGetWeathersService _service;
    public GetWeathersEndpoint(IGetWeathersService service)
    {
        _service = service;
    }

    /// <summary>
    /// APP 시작시 한번만 수행됨.
    /// </summary>
    public override void Configure()
    {
        Get("/api/weatherforecast/{pageno}/{pageSize}");
        Roles("Admin");
    }

    public override async Task HandleAsync(GetWeathersRequest req, CancellationToken ct)
    {
        this.Response = await _service.HandleAsync(req);
    }
}