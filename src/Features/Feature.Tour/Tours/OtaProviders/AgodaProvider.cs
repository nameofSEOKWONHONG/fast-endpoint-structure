using Feature.Domain.Tour.Dtos;

namespace Feature.Tour.Tours.OtaProviders;

public class AgodaProvider : IOtaProvider
{
    public Task<TourSummaryDto> GetTour(CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
}