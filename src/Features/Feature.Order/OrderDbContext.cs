using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Feature.Order;

public class OrderDbContext: DbContextBase<OrderDbContext>
{
    public OrderDbContext(DbContextOptions<OrderDbContext> options) : base(options)
    {
    }
    
    public DbSet<Entities.Order> Orders { get; set; }
}
