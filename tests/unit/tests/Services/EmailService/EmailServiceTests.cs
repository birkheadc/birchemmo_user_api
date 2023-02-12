using System.Threading.Tasks;
using BircheMmoUserApi.Services;
using BircheMmoUserApiUnitTests.Mocks.Config;
using BircheMmoUserApiUnitTests.Mocks.Services;
using Xunit;

namespace BircheMmoUserApiUnitTests.Services;

public class EmailServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    EmailService service = GetEmailServiceWithBadConfig();
    Assert.NotNull(service);
  }

  [Fact]
  public async Task SendEmailAsync_Returns_False_With_Bad_Config()
  {
    EmailService service = GetEmailServiceWithBadConfig();

    bool didSend = await service.SendEmailAsync("receiver_name", "receiver_address", "subject", "body");

    Assert.False(didSend);
  }

  private EmailService GetEmailServiceWithBadConfig()
  {
    return new EmailService(
      new MockEmailConfig_BadData(),
      new MockEmailVerificationService(),
      new MockEmailVerificationConfig_ReturnsGoodData()
    );
  }
}