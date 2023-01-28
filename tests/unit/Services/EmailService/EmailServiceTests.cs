using System.Threading.Tasks;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Config;
using Xunit;

namespace BircheMmoUserApiTests.Services;

public class EmailServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    EmailService service = new(new EmailConfigMock_BadData());
    Assert.NotNull(service);
  }

  [Fact]
  public async Task SendEmailAsync_Returns_False_With_Bad_Config()
  {
    EmailService service = new(new EmailConfigMock_BadData());

    bool didSend = await service.SendEmailAsync("receiver_name", "receiver_address", "subject", "body");

    Assert.False(didSend);
  }
}