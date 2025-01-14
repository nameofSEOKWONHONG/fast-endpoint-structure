using Infrastructure.Files;
using Infrastructure.KeyValueManager;
using Infrastructure.Session;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class InfrastructureExtensions
{
    public static void AddInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<ISessionUser, SessionUser>();
        services.AddScoped<ISessionDate, SessionDate>();
        services.AddScoped<ISessionContext, SessionContext>();
        services.AddSingleton<IKeyValueLoader, AwsSecretManagerLoader>();
        //services.AddSingleton<IKeyValueLoader, AzureKeyVaultLoader>();
        services.AddSingleton<KeyValueLoadExecutor>();

        //aws s3
        services.AddKeyedScoped<IFileService, S3FileService>("s3");
        //azure blob
        services.AddKeyedScoped<IFileService, BlobFileService>("blob");
    }

    public static void UseInfrastructure(this WebApplication app)
    {
        // using var scope = app.Services.CreateScope();
        // var resolver = scope.ServiceProvider.GetRequiredService<KeyValueLoadExecutor>();
        // resolver.Start(new Dictionary<string, string>()
        // {
        //     {"KEY_ID", Environment.GetEnvironmentVariable("AWS_KEYID")},
        //     {"ACCESS_KEY", Environment.GetEnvironmentVariable("AWS_ACCESSKEY")},
        //     {"REGION", Environment.GetEnvironmentVariable("AWS_REGION")},
        //     {"SECRET_NAME", Environment.GetEnvironmentVariable("AWS_SECRET_NAME")},
        //     {"VERSION_STAGE", "AWSCURRENT"},
        // });
    }
}