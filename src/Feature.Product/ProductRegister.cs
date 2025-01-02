﻿using Feature.Domain.Product.Abstract;
using Feature.Product.Core;
using Feature.Product.Plan.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Feature.Product;

public static class ProductRegister
{
    public static void AddProduct(this WebApplicationBuilder builder)
    {
        builder.Services.AddScoped<ICreatePlanService, CreatePlanService>();
        
        builder.Services
            .AddDbContext<ProductDbContext>((s, options) =>
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