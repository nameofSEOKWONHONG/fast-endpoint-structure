using Feature.Domain.Tour.Abstract;
using Feature.Tour.Tours.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Feature.Tour;

public static class DependencyInjection
{
    public static void AddWeatherFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IGetTourService, GetTourService>();
        builder.Services.AddScoped<IGatherTourBatchService, GatherTourBatchService>();
        builder.Services.AddTransient<IGatherTourService, GatherTourService>();
        
        builder.Services
            .AddDbContext<TourDbContext>((s, options) =>
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