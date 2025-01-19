using eXtensionSharp;
using Feature.Domain.Base;
using Feature.Domain.Product.Abstract;
using Feature.Domain.Product.Requests;
using Feature.Product.Plan.Entities;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Product.Plan.Services;


public class ModifyPlanService : ServiceBase<ModifyPlanService, ProductDbContext, PlanDto, Results<bool>>, IModifyPlanService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public ModifyPlanService(ILogger<ModifyPlanService> logger, ISessionContext sessionContext, ProductDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public override async Task<Results<bool>> HandleAsync(PlanDto request, CancellationToken cancellationToken)
    {
        var exists = await this.DbContext.ProductPlans.FirstOrDefaultAsync(m => m.Id == request.Id, cancellationToken: cancellationToken);
        if(exists.xIsEmpty()) return await Results<bool>.FailAsync("No product plan exists");
        
        exists.Title = request.Title;
        exists.Description = request.Description;
        exists.From = request.From;
        exists.To = request.To;
        exists.Step = request.Step;
        exists.Price = request.Price;
        exists.ModifiedBy = this.SessionContext.User.UserId;
        exists.ModifiedOn = this.SessionContext.Date.Now;
        
        this.DbContext.ProductPlans.Update(exists);
        await this.DbContext.SaveChangesAsync(cancellationToken);
        
        var approvalLines = await this.DbContext.ApprovalLines.Where(m => m.ProductPlanId == request.Id).ToListAsync(cancellationToken: cancellationToken);
        if(approvalLines.xIsEmpty()) return await Results<bool>.FailAsync("No approval lines exist");
        this.DbContext.ApprovalLines.RemoveRange(approvalLines);

        var list = approvalLines.Select(approvalLine => new PlanApprovalLine()
            {
                ProductPlanId = request.Id, UserId = approvalLine.UserId, CreatedBy = this.SessionContext.User.UserId, CreatedOn = this.SessionContext.Date.Now,
            })
            .ToList();

        this.DbContext.ApprovalLines.AddRange(list);
        await this.DbContext.SaveChangesAsync(cancellationToken);

        return await Results<bool>.SuccessAsync(true);
    }
}