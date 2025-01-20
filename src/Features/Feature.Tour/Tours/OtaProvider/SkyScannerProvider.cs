using Feature.Domain.Tour.Dtos;
using Feature.Tour.Tours.Entities;

namespace Feature.Tour.Tours.OtaProvider;

public class SkyScannerProvider : ITravelProvider
{
    public Task<TourSummaryDto> GetTour(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}