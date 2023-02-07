namespace BircheMmoUserApi.Services;

public interface IEmailService
{
  public Task SendVerificationEmail(string receiverName, string receiverAddress);
  public Task<bool> SendEmailAsync(string receiverName, string receiverAddress, string subject, string body);
}