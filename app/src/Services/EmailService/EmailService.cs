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

  public async Task SendVerificationEmail(string receiverName, string receiverAddress)
  {
    MimeMessage message = CreateVerificationMimeMessage(
      new MailboxAddress(
        receiverName,
        receiverAddress
      )
    );
    await SendMimeMessage(message);
  }

  private MimeMessage CreateVerificationMimeMessage(MailboxAddress receiver)
  {
    MimeMessage message = new();

    message.From.Add(new MailboxAddress(
      emailConfig.Name,
      emailConfig.Address
    ));
    message.To.Add(receiver);
    message.Subject = "Please verify your BircheGames account.";

    BodyBuilder bodyBuilder = new();

    string templatePath = "./assets/EmailVerificationTemplate/EmailVerificationTemplate.html";
    using (StreamReader reader = System.IO.File.OpenText(templatePath))
    {
      bodyBuilder.HtmlBody = reader.ReadToEnd();
    }

    message.Body = bodyBuilder.ToMessageBody();

    return message;
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