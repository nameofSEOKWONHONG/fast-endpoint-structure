using Feature.Domain.Base;
using Feature.Domain.Tour.Requests;
using Feature.Domain.Tour.Results;

namespace Feature.Domain.Tour.Abstract;

public interface IGetTourService : IServiceImpl<TourRequest, Results<IEnumerable<TourResult>>>
{
    
}