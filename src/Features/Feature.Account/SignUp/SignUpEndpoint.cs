using FastEndpoints;
using Feature.Account.Member.Services;
using Feature.Domain.Auth;
using Feature.Domain.Base;
using Feature.Domain.Member;

namespace Feature.Account.SignUp;

public class SignUpEndpoint : Endpoint<SignUpRequest, JResults<bool>>
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
        var result = await _service.HandleAsync(request, ct);
        this.Response = await JResults<bool>.SuccessAsync(result.Succeeded);
    }
}