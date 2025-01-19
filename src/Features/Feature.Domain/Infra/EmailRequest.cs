namespace Feature.Domain.Infra;

public class EmailRequest
{
    public string Subject { get; set; }
    public string Body { get; set; }
    public string FromName { get; set; }
    public string FromEmail { get; set; }
    public string ToName { get; set; }
    public string ToEmail { get; set; }
    public bool IsHtml { get; set; }
    public string Result { get; set; }
}
