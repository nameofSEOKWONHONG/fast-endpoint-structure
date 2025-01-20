using Feature.Domain.Base;
using Feature.Domain.Tour.Dtos;

namespace Feature.Domain.Tour.Abstract;

public interface IGatherTourService : IServiceImpl<string, TourSummaryDto>
{
    
}