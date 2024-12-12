﻿using Feature.Domain.Base;
using Feature.Domain.Weather.Request;
using Feature.Domain.Weather.Result;

namespace Feature.Domain.Weather.Abstract;

public interface IGetWeathersService
{
    Task<JPaginatedResult<GetWeatherResult>> HandleAsync(GetWeathersRequest request);
}