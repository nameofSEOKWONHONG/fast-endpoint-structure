namespace MovieSharpApi.Features.Weather.Domains;

public record WeatherForecastRequest(DateOnly Date, int TemperatureC, string? Summary);
