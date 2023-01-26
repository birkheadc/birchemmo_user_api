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

  public async Task SendTestEmailAsync(string recName, string recAddress)
  {
    Console.WriteLine("Log in using {0} : {1}", emailConfig.Username, emailConfig.Password);
    Console.WriteLine("Send message to {0} at {1}", recName, recAddress);
    EmailMessage message = new()
    {
      To = new MailboxAddress("Master Birkhead", "birkheadc@gmail.com"),
      Subject = "This is a test.",
      Content = "This is a test to see if I can send email!"
    };
    MimeMessage mimeMessage = CreateMimeMessage(message);
    await SendMimeMessage(mimeMessage);
  }

  private MimeMessage CreateMimeMessage(EmailMessage message)
  {
    MimeMessage mimeMessage = new();
    mimeMessage.From.Add(new MailboxAddress(emailConfig.Name, emailConfig.Address));
    mimeMessage.To.Add(message.To);
    mimeMessage.Subject = message.Subject;

    BodyBuilder bodyBuilder = new()
    {
      TextBody = message.Content
    };

    mimeMessage.Body = bodyBuilder.ToMessageBody();
    return mimeMessage;
  }

  private async Task SendMimeMessage(MimeMessage message)
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
      catch (Exception e)
      {
        Console.WriteLine("Failed to send email.");
        Console.WriteLine("Error: " + e.Message);
      }
      finally
      {
        await client.DisconnectAsync(true);
        client.Dispose();
      }
    }
  }
}