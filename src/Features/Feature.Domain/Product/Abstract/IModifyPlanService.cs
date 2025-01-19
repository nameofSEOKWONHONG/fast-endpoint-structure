using Feature.Domain.Base;
using Feature.Domain.Product.Requests;
using Feature.Domain.Weather.Abstract;

namespace Feature.Domain.Product.Abstract;

public interface IModifyPlanService : IServiceImpl<PlanDto, Results<bool>>
{
    
}
