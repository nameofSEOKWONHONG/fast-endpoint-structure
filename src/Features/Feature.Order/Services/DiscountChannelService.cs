using System.Net.Http.Json;
using eXtensionSharp;
using Feature.Domain.Base;
using Feature.Domain.Order.Abstract;
using Feature.Domain.Order.Reqeusts;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.Extensions.Logging;

namespace Feature.Order;





public class DiscountMemberService: ServiceBase<DiscountMemberService, DiscountChannelRequest, Results<double>>, IDiscountMemberService
{
    private readonly IHttpClientFactory _httpClientFactory;
    
    private readonly Dictionary<string, Func<DiscountChannelRequest, HttpClient, CancellationToken, Task<double>>> _discountStates =
        new()
        {
            {
                "discount-channel-a", async (request, client, cancellationToken) =>
                {
                    var res = await client.PostAsJsonAsync("api/discount/channel", request, cancellationToken);
                    res.EnsureSuccessStatusCode();
                    var result = await res.Content.ReadAsStringAsync(cancellationToken);
                    return result.xValue<double>();
                }
            },
            {
                "discount-channel-b", async (request, client, cancellationToken) =>
                {
                    var res = await client.PostAsJsonAsync("api/discount/channel", request, cancellationToken);
                    res.EnsureSuccessStatusCode();
                    var result = await res.Content.ReadAsStringAsync(cancellationToken);
                    return result.xValue<double>();
                }
            }
        };
    
    public DiscountMemberService(ILogger<DiscountMemberService> logger, ISessionContext sessionContext,
        IHttpClientFactory factory) : base(logger, sessionContext)
    {
        _httpClientFactory = factory;
    }

    public override async Task<Results<double>> HandleAsync(DiscountChannelRequest request,
        CancellationToken cancellationToken)
    {
        var result = await _discountStates[request.DiscountChannel](request, _httpClientFactory.CreateClient(request.DiscountChannel), cancellationToken);
        return await Results<double>.SuccessAsync(result);
    }
}