namespace Infrastructure.Session;

public interface ISessionContext
{
    ISessionUser User { get; set; }
    ISessionDate Date { get; set; }
}