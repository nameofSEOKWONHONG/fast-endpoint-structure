using FastEndpoints;
using FastEndpoints.Security;
using Feature.Account.Database;
using Feature.Weather.Database;
using Feature.Weather.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using FastEndpointApi.OpenApi;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    options.OpenApiVersion = OpenApiSpecVersion.OpenApi3_0;
    options.AddDocumentTransformer<OpenApiSecuritySchemeTransformer>();
});

//검증키
builder.Services
    .AddAuthenticationJwtBearer(s => s.SigningKey = "aHb2c9YRGSlFZDaPfbA3m4z+Lyr4O9dSWL2c+CiqWRM=")
    .AddAuthorization()
    .AddFastEndpoints()
    ;

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services
    .AddDbContext<AppDbContext>((s, options) =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SQL_CONNECTION"));
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging()
                .EnableThreadSafetyChecks()
                .EnableDetailedErrors()
                ;
        }
    });

builder.Services
    .AddDbContext<WeatherDbContext>((s, options) =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("SQL_CONNECTION"));
        if (builder.Environment.IsDevelopment())
        {
            options.EnableSensitiveDataLogging()
                .EnableThreadSafetyChecks()
                .EnableDetailedErrors()
                ;
        }
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app
        .MapScalarApiReference(options =>
        {
            options.WithOpenApiRoutePattern("/openapi/v1.json");
            options.Theme = ScalarTheme.None;
            options.Authentication = 
                new ScalarAuthenticationOptions
                {
                    PreferredSecurityScheme = FastEndpointApi.OpenApi.Constants.OpenApi.SecuritySchemeBearer
                };
        })
        .AllowAnonymous();
}

app.UseAuthentication() //add this
    .UseAuthorization() //add this
    .UseFastEndpoints()
    ;

app.UseHttpsRedirection();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IWeatherService>();
    await service.Initialize();    
}


app.Run();

