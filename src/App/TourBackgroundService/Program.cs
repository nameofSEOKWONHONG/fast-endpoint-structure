using TourBackgroundService;
using Feature.Tour;

DotNetEnv.Env.Load("./.env");

var builder = Host.CreateApplicationBuilder(args);
builder.Services.AddHostedService<GatherTourWorker>();
builder.Services.AddTourFeature(builder.Environment.IsDevelopment());

var host = builder.Build();
host.Run();