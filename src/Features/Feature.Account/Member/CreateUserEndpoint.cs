using eXtensionSharp;
using Feature.Account.Core;
using Feature.Account.Member.Services;
using Feature.Domain.Base;
using Feature.Domain.Member;
using Infrastructure.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Account.Member;

public class CreateUserEndpoint : JEndpoint<CreateUserEndpoint, CreateUserRequest, JResults<string>, AppDbContext>
{
    private readonly ICreateUserService _service;
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="dbContext"></param>
    public CreateUserEndpoint(ILogger<CreateUserEndpoint> logger, AppDbContext dbContext,
        ICreateUserService service) : base(logger, dbContext)
    {
        _service = service;
    }

    public override void Configure()
    {
        Post("/api/user/create");
        AllowAnonymous();
    }

    public override async Task HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        this.Response = await _service.HandleAsync(req, ct);
    }
}