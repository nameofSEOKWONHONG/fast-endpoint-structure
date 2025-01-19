using FastEndpoints;
using Feature.Domain.Product.Abstract;
using Feature.Domain.Product.Requests;
using Microsoft.Extensions.Logging;

namespace Feature.Product.Plan.Endpoints;

public class PlanCreateEndpoint : Endpoint<PlanDto>
{
    private readonly ICreatePlanService _service;
    private readonly ProductDbContext _dbContext;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="service"></param>
    /// <param name="context"></param>
    public PlanCreateEndpoint(ICreatePlanService service, ProductDbContext context)
    {
        _service = service;
        _dbContext = context;
    }
    
    public override void Configure()
    {
        Post("/api/product/plan");
    }

    public override async Task HandleAsync(PlanDto req, CancellationToken ct)
    {
        var tran = await _dbContext.Database.BeginTransactionAsync(ct);
        try
        {
            this.Response = await _service.HandleAsync(req, ct);
            await tran.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex, "{name} error: {message}", nameof(PlanCreateEndpoint), ex.Message);
            await tran.RollbackAsync(ct);
        }
    }
}