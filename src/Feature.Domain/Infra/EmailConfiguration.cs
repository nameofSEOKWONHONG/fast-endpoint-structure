using eXtensionSharp;

namespace Feature.Domain.Infra;

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