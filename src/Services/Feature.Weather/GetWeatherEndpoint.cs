﻿using FastEndpoints;
using Infrastructure.Domains;
using MovieSharpApi.Features.Weather.Domains;
using MovieSharpApi.Features.Weather.Services;

namespace MovieSharpApi.Features.Weather;

public class GetWeatherEndpoint : EndpointWithoutRequest<JResults<WeatherForecastResponse>>
{
    private readonly IWeatherService _weatherService;
    public GetWeatherEndpoint(IWeatherService weatherService)
    {
        _weatherService = weatherService;
    }
    
    public override void Configure()
    {
        Get("/api/weatherforecast/{id}");
        Roles("Manager");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        var id = Route<int>("id");
        var result = await _weatherService.GetWeatherForecast(id);
        this.Response = await JResults<WeatherForecastResponse>.SuccessAsync(result);
    }
}