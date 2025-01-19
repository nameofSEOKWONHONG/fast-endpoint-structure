namespace Feature.Domain.Base;

public interface IServiceImpl<in TRequest, TResult>
{
    Task<TResult> HandleAsync(TRequest request, CancellationToken ct);
}
