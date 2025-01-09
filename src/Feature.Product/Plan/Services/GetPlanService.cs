using Feature.Domain.Base;
using Feature.Domain.Product.Abstract;
using Feature.Domain.Product.Requests;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Product.Plan.Services;

public class GetPlanService : ServiceBase<GetPlanService, ProductDbContext, long, JResults<PlanDto>>, IGetPlanService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public GetPlanService(ILogger<GetPlanService> logger, ISessionContext sessionContext, ProductDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public override async Task<JResults<PlanDto>> HandleAsync(long request, CancellationToken cancellationToken)
    {
        var result = await this.DbContext.ProductPlans
            .Include(m => m.ApprovalLines)
            .Where(m => m.Id == request)
            .Select(m => new PlanDto()
            {
                Id = m.Id,
                Description = m.Description,
                Title = m.Title,
                Price = m.Price,
                From = m.From,
                To = m.To,
                Step = m.Step,
                ApprovalLines = m.ApprovalLines.Select(mm => new ApprovalLineDto(){ UserId = mm.UserId}).ToList()
            })
            .FirstOrDefaultAsync(m=> m.Id==request, cancellationToken);
        
        return await JResults<PlanDto>.SuccessAsync(result);
    }
}