using Feature.Domain.Tour.Abstract;
using Feature.Domain.Tour.Dtos;
using Feature.Tour.Tours.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Tour.Tours.Services;

public class GatherTourBatchService : ServiceBase<GatherTourBatchService, TourDbContext, string[], bool>, IGatherTourBatchService
{
    private readonly IGatherTourService _gatherTourServices;

    public GatherTourBatchService(ILogger<GatherTourBatchService> logger, ISessionContext sessionContext, TourDbContext dbContext,
        IGatherTourService gatherTourServices) : base(logger, sessionContext, dbContext)
    {
        _gatherTourServices = gatherTourServices;
    }

    public override async Task<bool> HandleAsync(string[] request, CancellationToken cancellationToken)
    {
        var list = new List<TourSummaryDto>();

        try
        {
            using var cts = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            cts.CancelAfter(TimeSpan.FromSeconds(30)); // 작업당 30초 제한
            var parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = Environment.ProcessorCount, // 적절히 설정
                CancellationToken = cts.Token
            };
            await Parallel.ForEachAsync(request, parallelOptions, async (item, token) =>
            {
                var summary = await _gatherTourServices.HandleAsync(item, token);
                list.AddRange(summary);
            });
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, e.Message);
        }

        var addItems = list.Select(m => new TourSummary()).ToList();
        await this.DbContext.TourSummaries.AddRangeAsync(addItems, cancellationToken);
        await this.DbContext.SaveChangesAsync(cancellationToken);

        return true;
    }
}