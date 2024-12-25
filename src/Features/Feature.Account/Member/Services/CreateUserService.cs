using eXtensionSharp;
using Feature.Account.Core;
using Feature.Domain.Base;
using Feature.Domain.Member;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Account.Member.Services;

public interface ICreateUserService
{
    Task<JResults<string>> HandleAsync(CreateUserRequest req, CancellationToken ct);
}

public class CreateUserService : ServiceBase<CreateUserService, AppDbContext>, ICreateUserService
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="logger"></param>
    /// <param name="sessionContext"></param>
    /// <param name="dbContext"></param>
    public CreateUserService(ILogger<CreateUserService> logger, ISessionContext sessionContext, AppDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public async Task<JResults<string>> HandleAsync(CreateUserRequest req, CancellationToken ct)
    {
        if(req.Password != req.ConfirmPassword) return await JResults<string>.SuccessAsync("Wrong input");
        
        var exists = await this.DbContext.Users.FirstOrDefaultAsync(m => m.Email == req.Email, ct);
        if(exists.xIsNotEmpty()) return await JResults<string>.SuccessAsync("Already registered");
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);
        var user = new Entities.User()
        {
            Id = Guid.CreateVersion7().ToString(),
            Email = req.Email,
            NormalizedEmail = req.Email.ToUpper(),
            PasswordHash = hashedPassword,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        
        await this.DbContext.Users.AddAsync(user, ct);
        await this.DbContext.SaveChangesAsync(ct);
        
        return await JResults<string>.SuccessAsync(user.Id);
    }
}