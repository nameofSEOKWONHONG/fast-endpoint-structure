using FastEndpoints;
using FastEndpoints.Security;
using Feature.Account.Database;
using Feature.Weather.Database;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;
using MovieSharpApi.Features.Auth.Entities;
using MovieSharpApi.Features.Weather.Services;
using MovieSharpApi.OpenApi;
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
    .AddAuthenticationJwtBearer(s => s.SigningKey = "aHb2c9YRGSlFZDaPfbA3m4z+Lyr4O9dSWL2c+CiqWRM=");
builder.Services
    .AddAuthorization()
    .AddFastEndpoints();
builder.Services.AddAuthentication(o =>
{
    o.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    o.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
});

builder.Services.AddIdentity<User, Role>(opt =>
{
    opt.Password.RequiredLength = 8;
    opt.Password.RequireDigit = false;
    opt.Password.RequireLowercase = false;
    opt.Password.RequireUppercase = false;
    opt.User.RequireUniqueEmail = true;
    opt.SignIn.RequireConfirmedAccount = false;
    opt.SignIn.RequireConfirmedEmail = false;
    opt.SignIn.RequireConfirmedPhoneNumber = false;
}).AddEntityFrameworkStores<AppDbContext>();

#pragma warning disable EXTEXP0018
builder.Services.AddHybridCache();
#pragma warning restore EXTEXP0018

builder.Services.AddScoped<IWeatherService, WeatherService>();

builder.Services
    .AddDbContext<AppDbContext>((s, options) =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
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
                    PreferredSecurityScheme = Constants.OpenApi.SecuritySchemeBearer
                };
        })
        .AllowAnonymous();
}

app.UseHttpsRedirection();

app.UseAuthentication() //add this
    .UseAuthorization() //add this
    .UseFastEndpoints();

using (var scope = app.Services.CreateScope())
{
    var service = scope.ServiceProvider.GetRequiredService<IWeatherService>();
    await service.Initialize();    
}


app.Run();

