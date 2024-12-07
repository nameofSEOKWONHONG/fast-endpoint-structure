using eXtensionSharp;
using FastEndpoints;
using Feature.Account.Core;
using Feature.Account.Entities;
using Feature.Account.SignUp.Domain;
using Infrastructure.Domains;
using Microsoft.EntityFrameworkCore;

namespace Feature.Account.SignUp;

public class SignUpEndpoint : Endpoint<SignUpRequest, JResults<bool>>
{
    private readonly AppDbContext _dbContext;

    public SignUpEndpoint(AppDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public override void Configure()
    {
        Post("/api/auth/signup");
        AllowAnonymous();
    }

    public override async Task HandleAsync(SignUpRequest req, CancellationToken ct)
    {
        if(req.Password != req.ConfirmPassword) this.Response = await JResults<bool>.SuccessAsync(false, "Wrong input");
        
        var exists = await _dbContext.Users.FirstOrDefaultAsync(m => m.Email == req.Email, ct);
        if(exists.xIsNotEmpty()) this.Response = await JResults<bool>.SuccessAsync(false, "Already registered");
        
        var hashedPassword = BCrypt.Net.BCrypt.HashPassword(req.Password);
        var user = new User()
        {
            Id = Guid.CreateVersion7().ToString(),
            Email = req.Email,
            NormalizedEmail = req.Email.ToUpper(),
            PasswordHash = hashedPassword,
            SecurityStamp = Guid.NewGuid().ToString(),
        };
        
        await _dbContext.Users.AddAsync(user, ct);
        await _dbContext.SaveChangesAsync(ct);
        
        this.Response = await JResults<bool>.SuccessAsync(true, "User registered");
    }
}