using eXtensionSharp;
using Feature.Domain.Base;
using Feature.Domain.Product.Abstract;
using Feature.Domain.Product.Requests;
using Feature.Domain.Weather.Abstract;
using Feature.Product.Core;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Product.Plan.Services;

public class GetListPlanService : ServiceBase<GetListPlanService, ProductDbContext, JPaginatedRequest<PlanSearchRequest>, JPaginatedResult<PlanDto>>, IGetListPlanService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public GetListPlanService(ILogger<GetListPlanService> logger, ISessionContext sessionContext, ProductDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }


    public override async Task<JPaginatedResult<PlanDto>> HandleAsync(JPaginatedRequest<PlanSearchRequest> request, CancellationToken cancellationToken)
    {
        var query = this.DbContext.ProductPlans.Include(m => m.ApprovalLines).AsQueryable();
        if (request.Data.Title.xIsNotEmpty())
        {
            query = query.Where(m => m.Title.Contains(request.Data.Title));
        }

        var totalCount = await query.CountAsync(cancellationToken);
        var result = await query
            .Skip(request.PageNo * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new PlanDto()
            {
                Id = m.Id,
                Title = m.Title,
                Description = m.Description,
                Price = m.Price,
                From = m.From,
                To = m.To,
                ApprovalLines = m.ApprovalLines.Select(mm => new ApprovalLineDto() { UserId = mm.UserId}).ToList()
            })
            .ToListAsync(cancellationToken: cancellationToken);
        
        return await JPaginatedResult<PlanDto>.SuccessAsync(result, totalCount, request.PageSize, totalCount);
    }
}