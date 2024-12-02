namespace Infrastructure.Session;

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