using Feature.Domain.Base;
using Feature.Domain.Order.Abstract;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Order.Services;

public class GetTaxService : ServiceBase<GetTaxService, string, Results<double>>, IGetTaxService
{
    public GetTaxService(ILogger<GetTaxService> logger, ISessionContext sessionContext) : base(logger, sessionContext)
    {
    }

    public override Task<Results<double>> HandleAsync(string request, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}
