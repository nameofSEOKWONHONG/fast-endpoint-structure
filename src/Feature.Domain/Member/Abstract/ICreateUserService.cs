using Feature.Domain.Base;

namespace Feature.Domain.Member.Abstract;

public interface ICreateUserService
{
    Task<JResults<string>> HandleAsync(CreateUserRequest req, CancellationToken ct);
}
