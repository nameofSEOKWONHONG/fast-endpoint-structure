using Feature.Domain.Order.Abstract;
using Feature.Order.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Feature.Order;

public static class DependencyInjection
{
    public static void AddOrderFeature(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<IOrderService, OrderService>();
        builder.Services.AddScoped<IGetItemQuantityService, GetItemQuantityService>();
        builder.Services.AddScoped<ISetItemQuantityService, SetItemQuantityService>();
        builder.Services.AddScoped<IBillingService, BillingService>();
        builder.Services.AddScoped<IGetTaxService, GetTaxService>();
        builder.Services.AddScoped<IGetDiscountService, GetDiscountService>();
        builder.Services.AddScoped<IDiscountMemberService, DiscountMemberService>();
        
        builder.Services
            .AddDbContext<OrderDbContext>((s, options) =>
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