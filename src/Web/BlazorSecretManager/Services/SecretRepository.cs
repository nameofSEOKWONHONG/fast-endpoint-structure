using BlazorSecretManager.Entities;
using eXtensionSharp;
using Microsoft.EntityFrameworkCore;

namespace BlazorSecretManager.Services;

public interface ISecretRepository
{
    Task<ValueTuple<int, List<Secret>>> GetSecrets(int pageNo = 1, int pageSize = 10);
    Task<Secret> GetSecret(int id);
    Task<int> CreateSecret(Secret secret);
    Task<bool> UpdateSecret(Secret secret);
    Task<bool> DeleteSecret(int id);
}

public class SecretRepository : ISecretRepository
{
    private readonly SecretDbContext _dbContext;
    public SecretRepository(SecretDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<ValueTuple<int, List<Secret>>> GetSecrets(int pageNo = 1, int pageSize = 10)
    {
        var query = _dbContext.Secrets.AsNoTracking();
        var total = await query.CountAsync();
        var list = await _dbContext.Secrets.AsNoTracking()
            .Skip((pageNo - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();
        
        return new ValueTuple<int, List<Secret>>(total, list);
    }

    public async Task<Secret> GetSecret(int id)
    {
        return await _dbContext.Secrets.AsNoTracking().FirstAsync(x => x.Id == id);
    }
    
    public async Task<int> CreateSecret(Secret secret)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == secret.Id);
        if (exists.xIsNotEmpty()) return 0;

        await _dbContext.Secrets.AddAsync(secret);
        await _dbContext.SaveChangesAsync();
        
        return secret.Id;
    }

    public async Task<bool> UpdateSecret(Secret secret)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == secret.Id);
        if (exists.xIsEmpty()) return false;
        
        _dbContext.Secrets.Update(secret);
        return await _dbContext.SaveChangesAsync() > 0;
    }

    public async Task<bool> DeleteSecret(int id)
    {
        var exists = await _dbContext.Secrets.FirstOrDefaultAsync(x => x.Id == id);
        if (exists.xIsEmpty()) return false;
        _dbContext.Secrets.Remove(exists);
        return await _dbContext.SaveChangesAsync() > 0;
    }
}