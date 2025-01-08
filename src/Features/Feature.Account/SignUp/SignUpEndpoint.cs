using FastEndpoints;
using Feature.Account.Member.Services;
using Feature.Domain.Auth;
using Feature.Domain.Base;
using Feature.Domain.Member;
using Feature.Domain.Member.Abstract;
using FluentValidation;
using FluentValidation.Results;

namespace Feature.Account.SignUp;

public class SignUpEndpoint : Endpoint<SignUpRequest, JResults<string>>
{
    private readonly ICreateUserService _service;

    public SignUpEndpoint(ICreateUserService service)
    {
        _service = service;
    }
    
    public override void Configure()
    {
        Post("/api/auth/signup");
        AllowAnonymous();
        Validator<SignUpRequestValidator>();
        DontThrowIfValidationFails();
    }

    public override async Task HandleAsync(SignUpRequest req, CancellationToken ct)
    {
        var request = new CreateUserRequest()
        {
            Email = req.Email,
            Password = req.Password,
            UserName = req.UserName,
            ConfirmPassword = req.ConfirmPassword,
        };
        
        if (ValidationFailed)
        {
            var map = ValidationFailures.Select(m => new KeyValuePair<string, string>(m.PropertyName, m.ErrorMessage)).ToDictionary();
            this.Response = await JResults<string>.FailAsync(map);
            return;
        }
        this.Response = await _service.HandleAsync(request, ct);
    }
}