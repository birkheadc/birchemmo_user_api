using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;
using Xunit;

public class UserServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    UserService userService = new(new UserRepository());
    Assert.NotNull(userService);
  }

  
}