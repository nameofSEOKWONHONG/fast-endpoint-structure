using Infrastructure.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddSessionContext(this IServiceCollection services)
    {
        services.AddScoped<ISessionUser, SessionUser>();
        services.AddScoped<ISessionCurrent, SessionCurrent>();
        services.AddScoped<ISessionContext, SessionContext>();
    }
}