using Confluent.Kafka;
using eXtensionSharp;
using Feature.Domain.Base;
using Feature.Domain.Infra;
using Feature.Domain.Infra.Abstract;
using Infrastructure.Base;
using Infrastructure.Session;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;

namespace Feature.Infra.Email;

public class EmailProducerService : ServiceBase<EmailProducerService, EmailRequest, JResults<bool>>, ISendEmailService
{
    private readonly IProducer<Null, string> _producer;
    public EmailProducerService(ILogger<EmailProducerService> logger, ISessionContext sessionContext, IProducer<Null, string> producer) : base(logger, sessionContext)
    {
        _producer = producer;
    }

    public override async Task<JResults<bool>> HandleAsync(EmailRequest request, CancellationToken cancellationToken)
    {
        var result = await _producer.ProduceAsync(TopicConstant.EmailTopic, new Message<Null, string> { Value = request.xToJson()}, cancellationToken);
        if (result.xIsNotEmpty())
        {
            this.Logger.LogInformation("{name} to topic {Topic}, partition {Partition}, offset {Offset}", nameof(EmailProducerService), result.Topic, result.Partition, result.Offset);                
        }

        return await JResults<bool>.SuccessAsync();
    }
}