using Feature.Domain.Tour.Dtos;

namespace Feature.Tour.Tours.OtaProviders;

public interface IOtaProvider
{
    Task<TourSummaryDto> GetTour(CancellationToken cancellationToken);
}