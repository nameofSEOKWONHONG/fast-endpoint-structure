﻿using Feature.Domain.Base;

namespace Feature.Domain.Weather.Request;

public class GetWeathersRequest : IPaginatedRequestBase
{
    public int PageNo { get; set; }
    public int PageSize { get; set; }
}