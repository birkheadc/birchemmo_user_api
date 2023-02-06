using System.Threading.Tasks;
using BircheMmoUserApi.Services;
using BircheMmoUserApiUnitTests.Mocks.Services;
using BircheMmoUserApiUnitTests.Mocks.Config;
using Xunit;
using BircheMmoUserApi.Repositories;
using MongoDB.Bson;
using BircheMmoUserApi.Models;

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
    UserService userService = await GetUserServiceWithUserAndPassword(username, password);
    EmailVerificationService service = new(userService, GetJwtTokenService());

    UserModel user = new(
      ObjectId.GenerateNewId(),
      "i_dont_exist",
      "hashed_password_is_not_actually_hashed_wow",
      "bad@user.model",
      Role.UNVALIDATED_USER,
      false
    );

    TokenWrapper? token = await service.GenerateEmailVerificationTokenForUser(user);
    Assert.Null(token);
  }

   [Fact]
  public async Task GenerateEmailVerificationTokenForUser_Returns_Token_If_User_Exists()
  {
    string username = "oldcheddar";
    string password = "password";
    UserService userService = await GetUserServiceWithUserAndPassword(username, password);
    EmailVerificationService service = new(userService, GetJwtTokenService());

    UserModel? user = await userService.GetUserByUsername(username);
    Assert.NotNull(user);

    TokenWrapper? token = await service.GenerateEmailVerificationTokenForUser(user);
    Assert.NotNull(token);
  }
  
  [Fact]
  public async Task GenerateEmailVerificationTokenForUser_Returns_Good_Token_If_User_Exists()
  {
    string username = "oldcheddar";
    string password = "password";
    UserService userService = await GetUserServiceWithUserAndPassword(username, password);
    EmailVerificationService service = new(userService, GetJwtTokenService());

    UserModel? user = await userService.GetUserByUsername(username);
    Assert.NotNull(user);

    TokenWrapper? token = await service.GenerateEmailVerificationTokenForUser(user);
    Assert.NotNull(token);

    bool isValid = await service.ValidateEmailVerificationTokenForUser(user, token);
    Assert.True(isValid);
  }

  private async Task<UserService> GetUserServiceWithUserAndPassword(string username, string password)
  {

    InMemoryUserRepository repository = new();

    await repository.CreateUser(new UserModel(
      ObjectId.GenerateNewId(),
      username,
      BCrypt.Net.BCrypt.HashPassword(password),
      username + "@site.com",
      Role.UNVALIDATED_USER,
      false
    ));

    return new UserService(repository);
  }

  private IJwtService GetJwtTokenService()
  {
    return new JwtService(new MockJwtConfig_GoodData());
  }
}