using BircheMmoUserApi.Controllers;
using BircheMmoUserApiUnitTests.Mocks.Services;
using Xunit;

namespace BircheMmoUserApiUnitTests.Controllers;

public class SessionControllerTests
{
  [Fact]
  public void Controller_Resolves()
  {
    SessionController controller = new(new MockSessionService_ReturnsBadData());

    Assert.NotNull(controller);
  }
}