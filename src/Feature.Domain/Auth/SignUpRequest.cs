using FastEndpoints;
using FluentValidation;

namespace Feature.Domain.Auth;

public class SignUpRequest
{
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string UserName { get; set; }
}

public class SignUpRequestValidator : Validator<SignUpRequest>
{
    public SignUpRequestValidator()
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