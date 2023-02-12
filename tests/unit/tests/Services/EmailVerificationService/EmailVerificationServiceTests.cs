using System.Threading.Tasks;
using BircheMmoUserApi.Services;
using BircheMmoUserApiUnitTests.Mocks.Services;
using BircheMmoUserApiUnitTests.Mocks.Config;
using Xunit;
using BircheMmoUserApi.Repositories;
using MongoDB.Bson;
using BircheMmoUserApi.Models;
using BircheMmoUserApiUnitTests.Mocks.Builders;

namespace BircheMmoUserApiUnitTests.Services;

public class EmailVerificationServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    EmailVerificationService service = new(new MockUserService_ReturnsGoodData(), GetJwtTokenService());

    Assert.NotNull(service);
  }

  [Fact]
  public async Task GenerateEmailVerificationTokenForUser_Returns_Null_If_User_Not_Found()
  {
    string username = "oldcheddar";
    string password = "password";
    UserService userService = await GetUserServiceWithCredentials(new Credentials(
      username,
      password
    ));
    EmailVerificationService service = new(userService, GetJwtTokenService());

    UserModel user = new MockUserModelBuilder()
      .Build();

    TokenWrapper? token = await service.GenerateForUser(user);
    Assert.Null(token);
  }

   [Fact]
  public async Task GenerateEmailVerificationTokenForUser_Returns_Token_If_User_Exists()
  {
    string username = "oldcheddar";
    string password = "password";
    UserService userService = await GetUserServiceWithCredentials(
      new Credentials(
        username,
        password
      )
    );
    EmailVerificationService service = new(userService, GetJwtTokenService());

    UserModel? user = await userService.GetUserByUsername(username);
    Assert.NotNull(user);

    TokenWrapper? token = await service.GenerateForUser(user);
    Assert.NotNull(token);
  }
  
  [Fact]
  public async Task GenerateEmailVerificationTokenForUser_Returns_Good_Token_If_User_Exists()
  {
    string username = "oldcheddar";
    string password = "password";
    UserService userService = await GetUserServiceWithCredentials(
      new Credentials(
        username,
        password
      )
    );
    EmailVerificationService service = new(userService, GetJwtTokenService());

    UserModel? user = await userService.GetUserByUsername(username);
    Assert.NotNull(user);
    user.UserDetails.Role = Role.UNVALIDATED_USER;

    TokenWrapper? token = await service.GenerateForUser(user);
    Assert.NotNull(token);

    bool isValid = await service.ValidateUser(token);
    Assert.True(isValid);
  }

  private async Task<UserService> GetUserServiceWithCredentials(Credentials credentials)
  {

    InMemoryUserRepository repository = new();

    await repository.CreateUser(
      new MockUserModelBuilder()
        .WithUsername(credentials.Username)
        .WithPassword(credentials.Password)
        .Build()
    );

    return new UserService(repository);
  }

  private IJwtService GetJwtTokenService()
  {
    return new JwtService(new MockJwtConfig_GoodData());
  }
}