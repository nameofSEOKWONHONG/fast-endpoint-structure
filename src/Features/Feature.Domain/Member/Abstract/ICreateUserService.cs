using Feature.Domain.Base;

namespace Feature.Domain.Member.Abstract;

public interface ICreateUserService
{
    Task<Results<string>> HandleAsync(CreateUserRequest req, CancellationToken ct);
}
