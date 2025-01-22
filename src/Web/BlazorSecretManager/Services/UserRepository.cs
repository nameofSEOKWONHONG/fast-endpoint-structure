using BlazorSecretManager.Entities;
using eXtensionSharp;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Services;

public interface IUserRepository
{
    Task<bool> CreateUser(User insertUser);
    Task<User> GetUser(string email);
}

public class UserRepository : IUserRepository
{
    private readonly SecretDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;

    public UserRepository(SecretDbContext dbContext,
        IPasswordHasher<User> passwordHasher)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
    }

    public async Task<bool> CreateUser(User insertUser)
    {
        var exists = await _dbContext.Users.FirstOrDefaultAsync(m => m.Email == insertUser.Email);
        if (exists.xIsNotEmpty()) return false;


        await _dbContext.Users.AddAsync(insertUser);
        await _dbContext.SaveChangesAsync();

        return true;
    }

    public async Task<User> GetUser(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(m => m.Email == email);
    }
}