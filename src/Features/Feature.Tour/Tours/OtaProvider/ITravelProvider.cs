using Feature.Domain.Tour.Dtos;
using Feature.Tour.Tours.Entities;

namespace Feature.Tour.Tours.OtaProvider;

public interface ITravelProvider
{
    Task<TourSummaryDto> GetTour(CancellationToken cancellationToken);
}