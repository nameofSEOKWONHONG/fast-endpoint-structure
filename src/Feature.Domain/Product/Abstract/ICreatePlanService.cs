using Feature.Domain.Base;

namespace Feature.Domain.Product.Abstract;

public interface ICreatePlanService
{
    Task<JResults<long>> HandleAsync(CreatePlanRequest request, CancellationToken ct);
}