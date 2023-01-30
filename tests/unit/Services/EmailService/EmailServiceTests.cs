using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Config;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace BircheMmoUserApiTests.Services;

public class EmailServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    EmailService service = new(new MockEmailConfig_BadData());
    Assert.NotNull(service);
  }

  [Fact]
  public async Task SendEmailAsync_Returns_False_With_Bad_Config()
  {
    EmailService service = new(new MockEmailConfig_BadData());

    bool didSend = await service.SendEmailAsync("receiver_name", "receiver_address", "subject", "body");

    Assert.False(didSend);
  }

  private IConfiguration GetConfiguration_BadEmailConfig()
  {
    Dictionary<string, string> badConfig = new()
    {
      {"Name", ""},
      {"Address", ""},
      {"SmtpServer", ""},
      {"Port", "-1"},
      {"Username", ""},
      {"Password", ""},
    };

    return new ConfigurationBuilder()
      .AddInMemoryCollection(badConfig)
      .Build();
  }
}