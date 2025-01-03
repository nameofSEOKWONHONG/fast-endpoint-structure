namespace Infrastructure.Session;

public class SessionDate : ISessionDate
{
    public DateTime Now => DateTime.UtcNow;

    private readonly ISessionUser _sessionUser;
    
    /// <summary>
    /// ctor
    /// </summary>
    /// <param name="user"></param>
    public SessionDate(ISessionUser user)
    {
        _sessionUser = user;
    }
}