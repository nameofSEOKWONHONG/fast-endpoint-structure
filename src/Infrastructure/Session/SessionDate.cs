namespace Infrastructure.Session;

public class SessionDate : ISessionDate
{
    public DateTime Now
    {
        get
        {
            return DateTime.UtcNow;
        }
    }

    private readonly ISessionUser _sessionUser;
    public SessionDate(ISessionUser user)
    {
        _sessionUser = user;
    }
}