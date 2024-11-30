using FastEndpoints;    

namespace Feature.Weather;

public class GetTestEndpoint : EndpointWithoutRequest
{
    public override void Configure()
    {
        Get("api/test");
        Roles("Admin");
    }

    public override async Task HandleAsync(CancellationToken ct)
    {
        await SendAsync("test", cancellation: ct);
    }
}