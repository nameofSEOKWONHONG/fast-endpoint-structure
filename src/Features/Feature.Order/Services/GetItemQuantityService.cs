using Feature.Domain.Base;
using Feature.Domain.Order.Abstract;
using Feature.Domain.Order.Reqeusts;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Order.Services;

public class GetItemQuantityService : ServiceBase<GetItemQuantityService>, IGetItemQuantityService
{
    public GetItemQuantityService(ILogger<GetItemQuantityService> logger, ISessionContext sessionContext) : base(logger, sessionContext)
    {
    }

    public Task<Results> HandleAsync(ItemQuantityRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}