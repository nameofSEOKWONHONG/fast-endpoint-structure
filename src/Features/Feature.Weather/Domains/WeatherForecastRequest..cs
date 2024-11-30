namespace Feature.Weather.Domains;

public record WeatherForecastRequest(DateOnly Date, int TemperatureC, string? Summary);
