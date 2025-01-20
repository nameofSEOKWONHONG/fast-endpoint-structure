using Feature.Domain.Tour.Abstract;
using Feature.Domain.Tour.Dtos;
using Feature.Tour.Tours.OtaProvider;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Tour.Tours.Services;

public class GatherTourService : ServiceBase<GatherTourService, TourDbContext, string, TourSummaryDto>, IGatherTourService
{
    private readonly IHttpClientFactory _httpClientFactory;

    public GatherTourService(ILogger<GatherTourService> logger, ISessionContext sessionContext, TourDbContext dbContext,
        IHttpClientFactory httpClientFactory) : base(logger, sessionContext, dbContext)
    {
        _httpClientFactory = httpClientFactory;
    }

    public override async Task<TourSummaryDto> HandleAsync(string request, CancellationToken cancellationToken)
    {
        var states = new Dictionary<string, Func<IHttpClientFactory, ITravelProvider>>()
        {
            {
                "hana", (clientFactory) =>
                {
                    var client = clientFactory.CreateClient("hana");
                    return new HanaProvider(client);
                }
            }
        };
        
        var provider = states[request](_httpClientFactory);
        return await provider.GetTour(cancellationToken);
    }
}