namespace Feature.Domain.Weather.Result;

public record GetWeatherResult(int Id, DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}