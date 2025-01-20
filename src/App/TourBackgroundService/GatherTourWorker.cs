using Feature.Domain.Tour.Abstract;

namespace TourBackgroundService;

public class GatherTourWorker : BackgroundService
{
    private readonly ILogger<GatherTourWorker> _logger;
    private readonly IGatherTourBatchService _gatherTourBatchService;

    public GatherTourWorker(ILogger<GatherTourWorker> logger, IGatherTourBatchService gatherTourBatchService)
    {
        _logger = logger;
        _gatherTourBatchService = gatherTourBatchService;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            if (_logger.IsEnabled(LogLevel.Information))
            {
                _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
            }

            var requests = new[]
            {
                "hana", "agoda"
            };
            await _gatherTourBatchService.HandleAsync(requests, stoppingToken);
            await Task.Delay(1000, stoppingToken);
        }
    }
}