using Microsoft.EntityFrameworkCore;

namespace Infrastructure.EF;

public abstract class DbContextBase<T> : DbContext
    where T : DbContext
{
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="options"></param>
    protected DbContextBase(DbContextOptions<T> options) : base(options)
    {
        
    }
    
    protected virtual void OnModelCreating(ModelBuilder modelBuilder, List<IEntityBuilderBase> entityBuilders)
    {
        foreach (var builder in entityBuilders)
        {
            builder.Build(modelBuilder);
        }
        
        base.OnModelCreating(modelBuilder);
    }
}