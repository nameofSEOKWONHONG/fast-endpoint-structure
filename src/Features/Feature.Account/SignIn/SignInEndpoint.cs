using eXtensionSharp;
using FastEndpoints;
using FastEndpoints.Security;
using Feature.Account.Core;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;

namespace Feature.Account.SignIn;

public class SignInEndpoint : Endpoint<LoginRequest>
{
    private readonly AppDbContext _dbContext;
    public SignInEndpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Post("/api/auth/signin");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(m => m.Email == req.Email, ct);
        if (user.xIsEmpty()) ThrowError("The supplied credentials are invalid!");

        var verify = BCrypt.Net.BCrypt.Verify(req.Password, user.PasswordHash);
        if(verify.xIsFalse()) ThrowError("The supplied credentials are invalid!");

        var jwtToken = JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = "aHb2c9YRGSlFZDaPfbA3m4z+Lyr4O9dSWL2c+CiqWRM=";
                o.ExpireAt = DateTime.UtcNow.AddDays(1);
                o.User.Roles.Add("Admin");
                o.User["UserId"] = user.Id;
                o.User["Email"] = user.Email;
                o.User["UserName"] = user.UserName;
            });

        await SendAsync(
            new
            {
                Token = jwtToken
            }, cancellation: ct);
    }
}