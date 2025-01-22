namespace BlazorSecretManager.Services.Auth;

public class UserSession
{
    public string UserId { get; set; }
    public string Email { get; set; }
    public string Name { get; set; }
    public string Role { get; set; }
}