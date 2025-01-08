using FastEndpoints;
using Feature.Domain.Auth;
using FluentValidation;

namespace Feature.Domain.Member;

public class CreateUserRequest : SignUpRequest
{
    public string Id { get; set; } 
}

public class CreateUserRequestValidator : Validator<CreateUserRequest>
{
    public CreateUserRequestValidator()
    {
        RuleFor(m => m.Email)
            .NotEmpty();
        RuleFor(m => m.Password)
            .NotEmpty();
        RuleFor(m => m.UserName)
            .NotEmpty();
        RuleFor(m => m.ConfirmPassword)
            .NotEmpty();
    }
}