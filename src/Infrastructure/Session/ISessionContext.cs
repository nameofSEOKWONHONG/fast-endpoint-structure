namespace Infrastructure.Session;

public interface ISessionContext
{
    ISessionUser User { get; set; }
    ISessionCurrent Current { get; set; }
}