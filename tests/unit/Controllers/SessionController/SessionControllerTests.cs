using BircheMmoUserApi.Controllers;
using BircheMmoUserApiTests.Mocks.Services;
using Xunit;

namespace BircheMmoUserApiTests.Controllers;

public class SessionControllerTests
{
  [Fact]
  public void Controller_Resolves()
  {
    SessionController controller = new(new MockSessionService_ReturnsGoodData());

    Assert.NotNull(controller);
  }
}