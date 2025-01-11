using eXtensionSharp;
using FastEndpoints;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace SecretManager;

public class SecretRequestBase
{
    public string Key { get; set; }
}

public class SecretDto : SecretRequestBase
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Description { get; set; }
    public string Json { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}
public class SecretRequest : SecretRequestBase
{
    public int Id { get; set; }
    public string Json { get; set; }
}

public class SecretSecurityProcess : IPreProcessor<SecretRequestBase>
{
    public Task PreProcessAsync(IPreProcessorContext<SecretRequestBase> context, CancellationToken ct)
    {
        if (!context.HttpContext.Request.Headers.TryGetValue("x-secret-key", out var secretKey))
        {
            context.ValidationFailures.Add(
                new("MissingHeaders", "The [x-tenant-id] header needs to be set!"));

            //sending response here
            return context.HttpContext.Response.SendErrorsAsync(context.ValidationFailures, cancellation: ct);
        }
        
        if(secretKey != Environment.GetEnvironmentVariable("SECRET_KEY"))
            return context.HttpContext.Response.SendForbiddenAsync(); //sending response here

        return Task.CompletedTask;
    }
}

public class TestEndpoint : EndpointWithoutRequest<string>
{
    public override void Configure()
    {
        AllowAnonymous();
        Get("/api/test");
    }

    public override Task HandleAsync(CancellationToken ct)
    {
        this.Response = "OK";
        return Task.CompletedTask;
    }
}

public class GetListSecretEndpoint : Endpoint<SecretDto, Results<Ok<string>, NotFound>>
{
    private readonly SecretDbContext _dbContext;
    public GetListSecretEndpoint(SecretDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("/api/secrets");
        AllowAnonymous();
        PreProcessor<SecretSecurityProcess>();
    }

    public override async Task HandleAsync(SecretDto req, CancellationToken ct)
    {
        var result = await _dbContext.Secrets.Select(m => new SecretDto
        {
            Id = m.Id, Title = m.Title, Description = m.Description, Json = m.Json, CreatedAt = m.CreatedAt,
            UpdatedAt = m.UpdatedAt
        }).ToListAsync(cancellationToken: ct);

        if (result.Count > 0)
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        
        await SendResultAsync(TypedResults.Ok(result));
    }
}

public class LoadSecretEndpoint : Endpoint<SecretRequest, Results<Ok<string>, NotFound>>
{
    private readonly SecretDbContext _dbContext;
    public LoadSecretEndpoint(SecretDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Get("/api/secret");
        AllowAnonymous();
        PreProcessor<SecretSecurityProcess>();
    }

    public override async Task HandleAsync(SecretRequest req, CancellationToken ct)
    {
        var exists = await _dbContext.Secrets
            .Where(m => m.Id == req.Id)
            .Select(m => new { m.Json })
            .FirstOrDefaultAsync(cancellationToken: ct);
        
        if (exists.xIsEmpty())
        {
            await SendResultAsync(TypedResults.NotFound());
            return;
        }
        
        await SendResultAsync(TypedResults.Ok(exists.Json));
    }
}

public class CreateSecretEndpoint : Endpoint<SecretDto, bool>
{
    private readonly SecretDbContext _dbContext;
    public CreateSecretEndpoint(SecretDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Post("/api/secret");
        AllowAnonymous();
        PreProcessor<SecretSecurityProcess>();
    }

    public override async Task HandleAsync(SecretDto req, CancellationToken ct)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(m => m.Id == req.Id, cancellationToken: ct);
        if (exists.xIsEmpty())
        {
            this.Response = false;
            return;
        }

        var secret = new SecretManager.Entities.Secret(req.Title, req.Description, req.Json, DateTime.Now, null);
        await _dbContext.Secrets.AddAsync(secret, ct);
        await _dbContext.SaveChangesAsync(ct);

        this.Response = true;
    }
}