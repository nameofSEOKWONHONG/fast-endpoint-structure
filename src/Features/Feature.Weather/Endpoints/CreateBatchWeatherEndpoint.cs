using eXtensionSharp;
using FastEndpoints;
using Feature.Domain.Base;
using Feature.Domain.Weather.Abstract;
using Feature.Domain.Weather.Request;
using Feature.Weather.Services;

namespace Feature.Weather.Endpoints;

public class CreateBatchWeatherRequest
{
    public WeatherForecastDto[] Items { get; set; }
}

public class CreateBatchWeatherEndpoint : Endpoint<CreateBatchWeatherRequest, JResults<int[]>> 
{
    private readonly WeatherDbContext _dbContext;
    private readonly ICreateWeatherService _weatherService;
    public CreateBatchWeatherEndpoint(WeatherDbContext context, ICreateWeatherService service)
    {
        _dbContext = context;
        _weatherService = service;
    }

    public override void Configure()
    {
        Post("api/weather/batch");
    }

    public override async Task HandleAsync(CreateBatchWeatherRequest req, CancellationToken ct)
    {
        var list = new List<JResults<int>>();
        var tran = await this._dbContext.Database.BeginTransactionAsync(ct);

        try
        {
            foreach (var item in req.Items)
            {
                var result = await this._weatherService.HandleAsync(item, ct);
                list.Add(result);
            }
            var fail = list.First(m => !m.Succeeded);
            if(fail.xIsEmpty()) await tran.CommitAsync(ct);

            //TODO : 반환 객체에 대해 변경해야 함.
            var items =list.Select(m => m.Data).ToArray();
            this.Response = await JResults<int[]>.SuccessAsync(items);
        }
        catch (Exception ex)
        {
            await tran.RollbackAsync(ct);
            this.Response = await JResults<int[]>.FailAsync(ex.Message);
        }
    }
}