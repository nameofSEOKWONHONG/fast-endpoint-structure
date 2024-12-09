using Feature.Domain.Weather.Request;
using Feature.Domain.Weather.Result;
using Infrastructure.Domains;

namespace Feature.Domain.Weather.Abstract;

public interface IGetWeathersService
{
    Task<JPaginatedResult<GetWeatherResult>> HandleAsync(GetWeathersRequest request);
}