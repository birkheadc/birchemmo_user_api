namespace BircheMmoUserApi.Services;

public interface IEmailService
{
  public Task<bool> SendEmailAsync(string receiverName, string receiverAddress, string subject, string body);
}