namespace Feature.Domain.Weather.Request;

public record CreateWeatherForecastRequest(DateOnly Date, int TemperatureC, string? Summary);