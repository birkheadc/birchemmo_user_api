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
    EmailService service = new(new MockEmailConfig_BadData(), new MockEmailVerificationService());
    Assert.NotNull(service);
  }

  [Fact]
  public async Task SendEmailAsync_Returns_False_With_Bad_Config()
  {
    EmailService service = new(new MockEmailConfig_BadData(), new MockEmailVerificationService());

    bool didSend = await service.SendEmailAsync("receiver_name", "receiver_address", "subject", "body");

    Assert.False(didSend);
  }
}