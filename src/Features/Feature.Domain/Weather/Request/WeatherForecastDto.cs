namespace Feature.Domain.Weather.Request;

public record WeatherForecastDto(int Id, DateOnly Date, int TemperatureC, string? Summary);