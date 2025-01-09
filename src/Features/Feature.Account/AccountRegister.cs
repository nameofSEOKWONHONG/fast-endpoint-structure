using eXtensionSharp;
using Feature.Account.Member.Services;
using Feature.Domain.Member.Abstract;
using Infrastructure.KeyValueManager;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Feature.Account;

public static class AccountRegister
{
    public static void AddAccountFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICreateUserService, CreateUserService>();
        
        builder.Services
            .AddDbContext<AppDbContext>((s, options) =>
            {
                //var loader = s.GetRequiredService<IKeyValueLoader>();
                //var con = loader.Data["SQL_CONNECTION"].xValue<string>();
                
                var con = Environment.GetEnvironmentVariable("SQL_CONNECTION");
                options.UseSqlServer(con);
                
                if (builder.Environment.IsDevelopment())
                {
                    options.EnableSensitiveDataLogging()
                        .EnableThreadSafetyChecks()
                        .EnableDetailedErrors()
                        ;
                }
            });
    }
}