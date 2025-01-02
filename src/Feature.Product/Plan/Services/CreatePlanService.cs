using eXtensionSharp;
using Feature.Domain.Base;
using Feature.Domain.Product;
using Feature.Domain.Product.Abstract;
using Feature.Product.Core;
using Feature.Product.Plan.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Riok.Mapperly.Abstractions;

namespace Feature.Product.Plan.Services;

public class CreatePlanService : ServiceBase<CreatePlanService, ProductDbContext>, ICreatePlanService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public CreatePlanService(ILogger<CreatePlanService> logger, ISessionContext sessionContext, ProductDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }


    public async Task<JResults<long>> HandleAsync(CreatePlanRequest request, CancellationToken ct)
    {
        var exists = await this.DbContext.ProductPlans.FirstOrDefaultAsync(m => m.Id == request.Id, ct);
        if(exists.xIsEmpty()) return await JResults<long>.FailAsync("No product plan exists");

        var addItem = request.RequestToPlan();

        await this.DbContext.ProductPlans.AddAsync(addItem!, ct);
        await this.DbContext.SaveChangesAsync(ct);

        var approvalLines = request.ApprovalLines
            .Select(line => new ApprovalLine()
            {
                ProductPlanId = addItem.Id, UserId = line.UserId, CreatedBy = SessionContext.User.UserId,
                CreatedOn = SessionContext.Date.Now
            })
            .ToList();
        await this.DbContext.ApprovalLines.AddRangeAsync(approvalLines, ct);
        await this.DbContext.SaveChangesAsync(ct);
        
        return await JResults<long>.SuccessAsync(addItem.Id);
    }
}

[Mapper]
public static partial class PlanMapper
{
    public static partial ProductPlan RequestToPlan(this CreatePlanRequest request);
}