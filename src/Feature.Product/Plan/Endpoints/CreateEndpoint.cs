using FastEndpoints;
using Feature.Domain.Product;
using Feature.Domain.Product.Abstract;
using Feature.Product.Core;

namespace Feature.Product.Plan.Endpoints;

public class CreateEndpoint : Endpoint<CreatePlanRequest>
{
    private readonly ICreatePlanService _service;
    private readonly ProductDbContext _dbContext;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="service"></param>
    /// <param name="context"></param>
    public CreateEndpoint(ICreatePlanService service, ProductDbContext context)
    {
        _service = service;
        _dbContext = context;
    }
    
    public override void Configure()
    {
        Post("/api/product/plan");
    }

    public override async Task HandleAsync(CreatePlanRequest req, CancellationToken ct)
    {
        var tran = await _dbContext.Database.BeginTransactionAsync(ct);
        try
        {
            this.Response = await _service.HandleAsync(req, ct);
            await tran.CommitAsync(ct);
        }
        catch (Exception ex)
        {
            await tran.RollbackAsync(ct);
        }
    }
}