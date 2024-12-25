using Feature.Domain.Auth;

namespace Feature.Domain.Member;

public class CreateUserRequest : SignUpRequest
{
    public string Id { get; set; } 
}