using Feature.Domain.Base;

namespace Feature.Domain.Infra.Abstract;

public interface ISendEmailService : IServiceImpl<EmailRequest, Results<bool>>
{
    
}
