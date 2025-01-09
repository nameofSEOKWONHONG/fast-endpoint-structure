using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Weather.Services;

public class RemoveWeatherService : ServiceRepoBase<RemoveWeatherService, IWeatherRepository, int, JResults<bool>>
{
    public RemoveWeatherService(ILogger<RemoveWeatherService> logger, ISessionContext sessionContext, IWeatherRepository repository) : base(logger, sessionContext, repository)
    {
    }

    public override async Task<JResults<bool>> HandleAsync(int request, CancellationToken cancellationToken)
    {
        var result = await this.Repository.Delete(request, cancellationToken);
        if(result) return await JResults<bool>.SuccessAsync();
        return await JResults<bool>.FailAsync();
    }
}