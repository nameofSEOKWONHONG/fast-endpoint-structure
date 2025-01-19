using Feature.Domain.Base;
using Feature.Domain.Order.Abstract;
using Feature.Domain.Order.Reqeusts;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Order.Services;

public class GetDiscountService : ServiceBase<GetDiscountService, DiscountChannelRequest, Results<double>>, IGetDiscountService
{
    private readonly IDiscountMemberService _discountMemberService;

    public GetDiscountService(ILogger<GetDiscountService> logger, ISessionContext sessionContext,
        IDiscountMemberService discountMemberService) : base(logger, sessionContext)
    {
        _discountMemberService = discountMemberService;
    }

    public override async Task<Results<double>> HandleAsync(DiscountChannelRequest request, CancellationToken cancellationToken)
    {
        var result = await _discountMemberService.HandleAsync(request, cancellationToken);
        return result;
    }
}