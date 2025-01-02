using eXtensionSharp.AspNet;
using FastEndpoints;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Session;

public class SessionPreProcess : IGlobalPreProcessor
{
    public Task PreProcessAsync(IPreProcessorContext ctx, CancellationToken ct)
    {
        if (ctx.HttpContext.xIsAuthenticated())
        {
            var session = ctx.HttpContext.RequestServices.GetRequiredService<ISessionContext>();
            var UserId = ctx.HttpContext.User.Claims.First(m => m.Type == "UserId").Value;
            var UserName = ctx.HttpContext.User.Claims.First(m => m.Type == "UserName").Value;
            var Email = ctx.HttpContext.User.Claims.First(m => m.Type == "Email").Value;        
            session.User = new SessionUser(UserId, UserName, Email);
            session.Date = new SessionDate(session.User);
        } 
        
        return Task.CompletedTask;
    }
}