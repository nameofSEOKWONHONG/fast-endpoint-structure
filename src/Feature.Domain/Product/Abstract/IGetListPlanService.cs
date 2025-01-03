using Feature.Domain.Base;
using Feature.Domain.Product.Requests;
using Feature.Domain.Weather.Abstract;

namespace Feature.Domain.Product.Abstract;

public interface IGetListPlanService : IServiceImpl<JPaginatedRequest<PlanSearchRequest>, JPaginatedResult<PlanDto>>
{
    
}
