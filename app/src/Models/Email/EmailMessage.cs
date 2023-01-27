using MimeKit;

namespace BircheMmoUserApi.Models;

public record EmailMessage
{
  public MailboxAddress? To { get; set; }
  public string? Subject { get; set; }
  public string? Content { get; set; }
  
}