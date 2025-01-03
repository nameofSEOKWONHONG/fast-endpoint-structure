using FastEndpoints;
using FastEndpoints.Security;
using Microsoft.OpenApi;
using FastEndpointApi.OpenApi;
using Feature.Account;
using Feature.Pdf;
using Feature.Product;
using Feature.Weather;
using Infrastructure;
using Infrastructure.Session;
using Scalar.AspNetCore;

DotNetEnv.Env.Load("./.env");

#region [service]

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

builder.Services.AddInfrastructure();
builder.AddWeatherFeature();
builder.AddAccountFeature();
builder.AddPdfFeature();
builder.AddProductFeature();

#endregion

#region [app]

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
    .UseFastEndpoints(c =>
    {
        c.Endpoints.Configurator = ep =>
        {
            if (!ep.AllowAnyClaim || !ep.AllowAnyPermission)
            {
                ep.PreProcessor<SessionPreProcess>(Order.Before);    
            }
        };
    })
    ;

app.UseWeatherFeature();

app.UseHttpsRedirection();

app.UseInfrastructure();

app.Run();

#endregion
