using eXtensionSharp;
using Feature.Domain.Base;
using Feature.Domain.Tour.Abstract;
using Feature.Domain.Tour.Requests;
using Feature.Domain.Tour.Results;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Feature.Tour.Tours.Services;

public class GetTourService : ServiceBase<GetTourService, TourDbContext, TourRequest, Results<IEnumerable<TourResult>>>, IGetTourService
{
    public GetTourService(ILogger<GetTourService> logger, ISessionContext sessionContext, TourDbContext dbContext) : base(logger, sessionContext, dbContext)
    {
    }

    public override async Task<Results<IEnumerable<TourResult>>> HandleAsync(TourRequest request, CancellationToken cancellationToken)
    {
        var from = request.StartDate.xFromDate();
        var to = request.EndDate.xToDate();
        var query = this.DbContext.TourSummaries.AsNoTracking()
            .Where(m => m.Date >= from && m.Date < to)
            .Where(m => m.Nation.Contains(request.Nation));

        if (request.Region.xIsNotEmpty())
        {
            query = query.Where(m => m.Region.Contains(request.Region));
        }

        if (request.City.xIsNotEmpty())
        {
            query = query.Where(m => m.City.Contains(request.City));
        }
        
        var tourSummaries = await query
            .Skip(request.PageNo * request.PageSize)
            .Take(request.PageSize)
            .Select(m => new TourResult()
            {
                Id = m.Id,
                Date = m.Date,
                Nation = m.Nation,
                City = m.City,
                Region = m.Region,
                TourId = m.TourId
            })
            .ToListAsync(cancellationToken);
        return await Results<IEnumerable<TourResult>>.SuccessAsync(tourSummaries);
    }
}