using System.Transactions;
using eXtensionSharp;
using Feature.Domain.Base;
using Feature.Domain.Order.Abstract;
using Feature.Domain.Order.Reqeusts;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Order.Services;

public class OrderService : ServiceBase<IOrderService, OrderDbContext, OrderRequest, Results>, IOrderService
{
    private readonly IGetItemQuantityService _getItemQuantityService;
    private readonly ISetItemQuantityService _setItemQuantityService;
    private readonly IGetTaxService _getTaxService;
    private readonly IGetDiscountService _getDiscountService;
    private readonly IBillingService _billingService;

    public OrderService(ILogger<IOrderService> logger, ISessionContext sessionContext, OrderDbContext dbContext,
        IGetItemQuantityService getItemQuantityService,
        ISetItemQuantityService setItemQuantityService,
        IGetTaxService getTaxService,
        IGetDiscountService getDiscountService,
        IBillingService billingService) : base(logger, sessionContext, dbContext)
    {
        _getItemQuantityService = getItemQuantityService;
        _setItemQuantityService = setItemQuantityService;
        _getTaxService = getTaxService;
        _getDiscountService = getDiscountService;
        _billingService = billingService;
    }

    public override async Task<Results> HandleAsync(OrderRequest request, CancellationToken ct)
    {
        using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);
        var newOrder = this.DbContext.Orders.FirstOrDefault(m => m.OrderGuid == request.OrderGuid);
        if (newOrder.xIsNotEmpty()) return await Results.FailAsync();

        await this.DbContext.Orders.AddAsync(new Entities.Order()
        {
            OrderGuid = request.OrderGuid, CreatedBy = this.SessionContext.User.UserId,
            CreatedOn = this.SessionContext.Date.Now
        }, ct);
        await this.DbContext.SaveChangesAsync(ct);
        
        var total = 0;
        foreach (var item in request.OrderItems)
        {
            var requestItem = new ItemQuantityRequest() { ItemId = item.ItemId, Quantity = item.Quantity };
            var exists = await _getItemQuantityService.HandleAsync(requestItem, ct);
            if (exists.Succeeded.xIsFalse()) return exists;

            var setResult = await _setItemQuantityService.HandleAsync(requestItem, ct);
            if (setResult.Succeeded.xIsFalse()) return setResult;
            
            total += item.Quantity;
        }

        var tax = await _getTaxService.HandleAsync(string.Empty, ct);
        if(tax.Succeeded.xIsFalse()) return tax;
        
        var discount = await _getDiscountService.HandleAsync(new DiscountChannelRequest()
        {
            UserId = this.SessionContext.User.UserId,
            DiscountChannel = "discount-channel-a",
        }, ct);
        if(discount.Succeeded.xIsFalse()) return tax;

        var billing = new BillingRequest()
        {
            CustomerNo = this.SessionContext.User.UserId,
            OrderId = newOrder.OrderId,
            Total = total,
            Tax = total * tax.Data,
            Discount = discount.Data
        };
        var result = await _billingService.HandleAsync(billing, ct);
        if (result.Succeeded.xIsFalse()) return await Results.FailAsync();
        
        scope.Complete();

        return result;
    }
}
