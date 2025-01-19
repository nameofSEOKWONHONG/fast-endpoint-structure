using Feature.Domain.Base;
using Feature.Domain.Order.Abstract;
using Feature.Domain.Order.Reqeusts;

namespace Feature.Order.Services;

public class BillingService : IBillingService
{
    public Task<Results> HandleAsync(BillingRequest request, CancellationToken ct)
    {
        throw new NotImplementedException();
    }
}