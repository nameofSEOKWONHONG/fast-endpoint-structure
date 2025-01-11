using Confluent.Kafka;
using eXtensionSharp;
using Feature.Domain.Base;
using Infrastructure.Base;
using Infrastructure.Extensions;
using Infrastructure.Session;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace Feature.Infra.Email;

public class EmailConfiguration
{
    /// <summary>
    /// 발신자 Email
    /// </summary>
    public string From { get; private set; } = Environment.GetEnvironmentVariable("SMTP_FROM");

    /// <summary>
    /// 발신자 표시명
    /// </summary>
    public string DisplayName { get; private set; } = Environment.GetEnvironmentVariable("SMTP_DISPLAYNAME");
    /// <summary>
    /// SMTP Host
    /// </summary>
    public string Host { get; private set; } = Environment.GetEnvironmentVariable("SMTP_HOST");

    /// <summary>
    /// Port
    /// </summary>
    public int Port { get; private set; } = Environment.GetEnvironmentVariable("SMTP_PORT").xValue<int>();

    /// <summary>
    /// 계정명
    /// </summary>
    public string UserName { get; private set; } = Environment.GetEnvironmentVariable("SMTP_USERNAME");

    /// <summary>
    /// 계정암호
    /// </summary>
    public string Password { get; private set; } = Environment.GetEnvironmentVariable("SMTP_PASSWORD");
}

public interface IEmailConsumerService : IServiceImpl<bool, bool>
{
    
}

public class EmailConsumerService : ServiceBase<EmailConsumerService>, IEmailConsumerService
{
    private readonly IConsumer<Null, string> _consumer;
    private readonly EmailConfiguration _emailConfiguration = new();
    
    public EmailConsumerService(ILogger<EmailConsumerService> logger, ISessionContext sessionContext, IConsumer<Null, string> consumer) : base(logger, sessionContext)
    {
        _consumer = consumer;
    }


    public async Task<bool> HandleAsync(bool req, CancellationToken ct)
    {
        var consumeResult = _consumer.Consume(ct);

        try
        {
            var request = consumeResult.Message.Value.ToDeserialize<EmailRequest>();
            if (!Validate(request))
            {
                this.Logger.LogWarning("{name} email validate failed: {message}", nameof(EmailConsumerService), request.xToJson());
                return false;                
            }
            
            var statusMessage = await SendEmail(_emailConfiguration, request.FromName, request.FromEmail,
                request.ToName, request.ToEmail, request.Subject, request.Body, request.IsHtml);
                
            this.Logger.LogInformation("{name} send email: {message}", nameof(EmailConsumerService), statusMessage);
        }
        catch (Exception e)
        {
            this.Logger.LogError(e, "{name} error: {message}", nameof(EmailConsumerService), e.Message);
            return false;
        }
        finally
        {
            _consumer.Commit(consumeResult);
        } 

        return true;
    }
    
    private static bool Validate(EmailRequest obj)
    {
        if (obj.xIsEmpty()) return false;
        
        return obj.FromEmail.xIsNotEmpty() &&
               obj.FromName.xIsNotEmpty() &&
               obj.ToEmail.xIsNotEmpty() &&
               obj.ToName.xIsNotEmpty() &&
               obj.Subject.xIsNotEmpty() &&
               obj.Body.xIsNotEmpty();
    }
    
    private static MimeMessage CreateMessage(string fromName, string fromEmail, string toName, string toEmail, string subject, string body, bool isHtml)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(fromName, fromEmail));
        message.To.Add(new MailboxAddress(toName, toEmail));
        message.Subject = subject;
        message.Body = isHtml switch
        {
            true => new BodyBuilder { HtmlBody = body }.ToMessageBody(),
            false => new BodyBuilder { TextBody = body }.ToMessageBody()
        };
        
        return message;
    }
    
    private static async Task<string> SendEmail(EmailConfiguration config, string fromName, string fromEmail, string toName, string toEmail, string subject, string body, bool isHtml)
    {
        var message = CreateMessage(fromName.xValue<string>(config.DisplayName), fromEmail.xValue<string>(config.From), toName, toEmail, subject, body, isHtml);
        using var smtp = new SmtpClient();
        await smtp.ConnectAsync(config.Host, config.Port, true);
        await smtp.AuthenticateAsync(config.UserName, config.Password);
        var result = await smtp.SendAsync(message);
        await smtp.DisconnectAsync(true);

        return result;
    }
}