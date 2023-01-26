namespace BircheMmoUserApi.Services;

public interface IEmailService
{
  public Task SendTestEmailAsync(string recName, string recAddress);
}