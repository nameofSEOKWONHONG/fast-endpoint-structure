using Feature.Domain.Base;

namespace Feature.Domain.Order.Abstract;

public interface IGetTaxService : IServiceImpl<string, Results<double>>
{
    
}