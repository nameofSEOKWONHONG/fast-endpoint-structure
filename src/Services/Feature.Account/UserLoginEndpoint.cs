using eXtensionSharp;
using FastEndpoints;
using FastEndpoints.Security;
using Feature.Account.Database;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using MovieSharpApi.Features.Auth.Entities;

namespace MovieSharpApi.Features.Auth;

public class UserLoginEndpoint : Endpoint<LoginRequest>
{
    private readonly AppDbContext _dbContext;
    private readonly IPasswordHasher<User> _hasher;
    public UserLoginEndpoint(AppDbContext dbContext, IPasswordHasher<User> hasher)
    {
        _dbContext = dbContext;
        _hasher = hasher;
    }
    
    public override void Configure()
    {
        Post("/api/auth/login");
        AllowAnonymous();
    }

    public override async Task HandleAsync(LoginRequest req, CancellationToken ct)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(m => m.Email == req.Email, ct);
        if (user.xIsEmpty()) ThrowError("The supplied credentials are invalid!");
        
        var verify = _hasher.VerifyHashedPassword(user, user.PasswordHash, req.Password);
        if(verify == PasswordVerificationResult.Failed) ThrowError("The supplied credentials are invalid!");

        var jwtToken = JwtBearer.CreateToken(
            o =>
            {
                o.SigningKey = "aHb2c9YRGSlFZDaPfbA3m4z+Lyr4O9dSWL2c+CiqWRM=";
                o.ExpireAt = DateTime.UtcNow.AddDays(1);
                o.User.Roles.Add("Admin");
                o.User.Claims.Add(("UserName", "tester"));
                o.User["UserId"] = user.Id;
                o.User["Email"] = user.Email;
            });

        await SendAsync(
            new
            {
                UserName = "tester",
                Token = jwtToken
            }, cancellation: ct);
    }
}