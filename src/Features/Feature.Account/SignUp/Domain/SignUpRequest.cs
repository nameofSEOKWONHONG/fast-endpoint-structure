namespace Feature.Account.SignUp.Domain;

public class SignUpRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
}