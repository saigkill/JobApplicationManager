namespace JobApplicationManager.Infrastructure.Models;

public class EmailModel
{
    public string ContactName { get; set; } = string.Empty;
    public string CompEmail { get; set; } = string.Empty;
    public string Subject { get; set; } = string.Empty;
    public string Salutation { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public string Attachment { get; set; } = string.Empty;
}
