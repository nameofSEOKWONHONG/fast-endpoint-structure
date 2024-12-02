using Infrastructure.KeyValueManager;
using Infrastructure.Session;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISessionUser, SessionUser>();
        services.AddScoped<ISessionCurrent, SessionCurrent>();
        services.AddScoped<ISessionContext, SessionContext>();
        services.AddSingleton<IKeyValueLoader, AwsSecretManagerLoader>();
        //services.AddSingleton<IKeyValueLoader, AzureKeyVaultLoader>();
        services.AddSingleton<KeyValueLoadExecutor>();
    }
}