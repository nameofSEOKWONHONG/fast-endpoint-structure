namespace Infrastructure.Session;

public class SessionUser : ISessionUser
{
    public string UserId { get; }
    public string UserName { get; }
    public string Email { get; }

    public SessionUser()
    {
        
    }
    
    public SessionUser(string userId, string userName, string email)
    {
        this.UserId = userId;
        this.UserName = userName;
        this.Email = email;
    }
}