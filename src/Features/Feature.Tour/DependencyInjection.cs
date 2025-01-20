using Feature.Domain.Tour.Abstract;
using Feature.Tour.Tours.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Feature.Tour;

public static class DependencyInjection
{
    public static void AddTourFeature(this IServiceCollection services, bool isDevelopment)
    {
        services.AddHttpClient("hana", client =>
        {
            client.BaseAddress = new Uri("https://api.hanatour.com");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorize", Environment.GetEnvironmentVariable("HANA_TOUR_API_KEY"));
        });
        services.AddHttpClient("agoda", client =>
        {
            client.BaseAddress = new Uri("https://api.agoda.com");
            client.DefaultRequestHeaders.Add("Accept", "application/json");
            client.DefaultRequestHeaders.Add("Authorize", Environment.GetEnvironmentVariable("AGODA_TOUR_API_KEY"));            
        });
        services.AddScoped<IGetTourService, GetTourService>();
        services.AddScoped<IGatherTourBatchService, GatherTourBatchService>();
        services.AddScoped<IGatherTourService, GatherTourService>();
        
        services.AddDbContext<TourDbContext>((s, options) =>
            {
                //var loader = s.GetRequiredService<IKeyValueLoader>();
                //var con = loader.Data["SQL_CONNECTION"].xValue<string>();
                
                var con = Environment.GetEnvironmentVariable("SQL_CONNECTION");
                options.UseSqlServer(con);
                if (isDevelopment)
                {
                    options.EnableSensitiveDataLogging()
                        .EnableThreadSafetyChecks()
                        .EnableDetailedErrors()
                        ;
                }
            });
    }
}