﻿using Feature.Domain.Base;
using Feature.Domain.Order.Reqeusts;

namespace Feature.Domain.Order.Abstract;

public interface ISetItemQuantityService: IServiceImpl<ItemQuantityRequest, Results>
{
    
}