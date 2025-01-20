using Feature.Tour.Tours.Entities;
using Infrastructure.EF;
using Microsoft.EntityFrameworkCore;

namespace Feature.Tour;

public class TourDbContext : DbContextBase<TourDbContext>
{
    public TourDbContext(DbContextOptions<TourDbContext> options) : base(options)
    {
    }
    
    public DbSet<Tours.Entities.Tour> Tours { get; set; }
    public DbSet<TourDetail> TourDetails { get; set; }
    public DbSet<TourSummary> TourSummaries { get; set; }
}

public enum TourType
{
    Inner,
    Outer,
}