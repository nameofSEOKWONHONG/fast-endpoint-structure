namespace Infrastructure.Session;

public class SessionContext : ISessionContext
{
    public ISessionUser User { get; set; }
    public ISessionDate Date { get; set; }
}