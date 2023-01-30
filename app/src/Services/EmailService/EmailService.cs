using BircheMmoUserApi.Config;
using BircheMmoUserApi.Models;
using MailKit.Net.Smtp;
using MimeKit;

namespace BircheMmoUserApi.Services;

public class EmailService : IEmailService
{
  private readonly EmailConfig emailConfig;

  public EmailService(EmailConfig emailConfig)
  {
    this.emailConfig = emailConfig;
  }

  public async Task<bool> SendEmailAsync(string receiverName, string receiverAddress, string subject, string body)
  {
    EmailMessage message = new()
    {
      To = new MailboxAddress(receiverName, receiverAddress),
      Subject = subject,
      Body = body
    };
    MimeMessage mimeMessage = CreateMimeMessage(message);
    bool didSend = await SendMimeMessage(mimeMessage);
    return didSend;
  }

  public void Test()
  {
    Console.WriteLine("Address: " + emailConfig.Address);
    Console.WriteLine("Username: " + emailConfig.Username);    
  }

  private MimeMessage CreateMimeMessage(EmailMessage message)
  {
    MimeMessage mimeMessage = new();
    mimeMessage.From.Add(new MailboxAddress(emailConfig.Name, emailConfig.Address));
    mimeMessage.To.Add(message.To);
    mimeMessage.Subject = message.Subject;

    BodyBuilder bodyBuilder = new()
    {
      TextBody = message.Body
    };

    mimeMessage.Body = bodyBuilder.ToMessageBody();
    return mimeMessage;
  }

  private async Task<bool> SendMimeMessage(MimeMessage message)
  {
    using (SmtpClient client = new())
    {
      try
      {
        Console.WriteLine("Attempting to connect to email...");
        await client.ConnectAsync(emailConfig.SmtpServer, emailConfig.Port, MailKit.Security.SecureSocketOptions.StartTls);
        Console.WriteLine("Autenticating...");
        await client.AuthenticateAsync(emailConfig.Username, emailConfig.Password);
        Console.WriteLine("Sending...");
        await client.SendAsync(message);
        Console.WriteLine("Sent successfully!");
      }
      catch
      {
        Console.WriteLine("Failed to send email.");
        return false;
      }
      finally
      {
        await client.DisconnectAsync(true);
        client.Dispose();
      }
      return true;
    }
  }
}