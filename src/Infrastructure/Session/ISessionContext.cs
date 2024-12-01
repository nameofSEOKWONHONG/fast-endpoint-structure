using System.Security.Claims;
using eXtensionSharp.AspNet;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Session;

public interface ISessionContext
{
    ISessionUser User { get; }
    ISessionCurrent Current { get; }
}

public interface ISessionContext<T>
    where T : DbContext
{
    T DbContext { get; }
}

public class SessionContext : ISessionContext
{
    public ISessionUser User { get; }
    public ISessionCurrent Current { get; }
}

public class SessionContext<T> : ISessionContext<T>
    where T : DbContext
{
    public T DbContext { get; }
    public ISessionUser User { get; }
    public ISessionCurrent Current { get; }
}

public interface ISessionUser
{
    string UserId { get; }
    string UserName { get; }
    string Email { get; }
}

public class SessionUser : ISessionUser
{
    public string UserId { get; }
    public string UserName { get; }
    public string Email { get; }

    public SessionUser(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext.xIsAuthenticated())
        {
            UserId = httpContextAccessor.HttpContext.User.Claims.First(m => m.Type == ClaimTypes.NameIdentifier).Value;
            UserName = httpContextAccessor.HttpContext.User.Claims.First(m => m.Type == ClaimTypes.Name).Value;
            Email = httpContextAccessor.HttpContext.User.Claims.First(m => m.Type == ClaimTypes.Email).Value;
        }
    }
}

public interface ISessionCurrent
{
    DateTime Now { get; }
}

public class SessionCurrent : ISessionCurrent
{
    public DateTime Now
    {
        get
        {
            return DateTime.UtcNow;
        }
    }

    private readonly ISessionUser _sessionUser;
    public SessionCurrent(ISessionUser user)
    {
        _sessionUser = user;
    }
}