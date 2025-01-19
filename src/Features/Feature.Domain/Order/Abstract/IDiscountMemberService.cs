using Feature.Domain.Base;
using Feature.Domain.Order.Reqeusts;

namespace Feature.Domain.Order.Abstract;

public interface IDiscountMemberService: IServiceImpl<DiscountChannelRequest, Results<double>>
{
    
}