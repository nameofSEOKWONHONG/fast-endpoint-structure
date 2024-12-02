namespace Infrastructure.Session;

public interface ISessionUser
{
    string UserId { get; }
    string UserName { get; }
    string Email { get; }
}