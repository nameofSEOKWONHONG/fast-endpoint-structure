using Feature.Domain.Base;
using Feature.Domain.Order.Abstract;
using Feature.Domain.Order.Reqeusts;

namespace Feature.Order.Services;

public class SetItemQuantityService : ISetItemQuantityService
{
    public Task<Results> HandleAsync(ItemQuantityRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}
