using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;
using BircheMmoUserApiUnitTests.Mocks.Services;
using BircheMmoUserApiUnitTests.Mocks.Config;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiUnitTests.Services;

#pragma warning disable 1998
public class SessionServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    SessionService service = new(
      new MockUserService_ReturnsGoodData(),
      GetJwtTokenService()
    );

    Assert.NotNull(service);
  }

  [Fact]
  public async Task GenerateSessionToken_Returns_Null_With_Bad_Credentials()
  {
    string username = "oldcheddar";
    string password = "password";

    UserService userService = await GetUserServiceWithUserAndPassword(username, password);

    Assert.NotNull(await userService.GetUserByUsername(username));

    SessionService service = new(
      userService,
      GetJwtTokenService()
    );
    
    TokenWrapper? token = await service.GenerateSessionToken(new Credentials(
      username,
      "badpassword"
    ));

    Assert.Null(token);
  }

  [Fact]
  public async Task GenerateSessionToken_Returns_Token_With_Good_Credentials()
  {
    string username = "oldcheddar";
    string password = "password";

    UserService userService = await GetUserServiceWithUserAndPassword(username, password);

    Assert.NotNull(await userService.GetUserByUsername(username));

    SessionService service = new(
      userService,
      GetJwtTokenService()
    );
    
    TokenWrapper? token = await service.GenerateSessionToken(new Credentials(
      username,
      password
    ));

    Assert.NotNull(token);
  }

  [Fact]
  public async Task ValidateSessionToken_Returns_Null_With_Bad_Token()
  {
    string username = "oldcheddar";
    string password = "password";

    UserService userService = await GetUserServiceWithUserAndPassword(username, password);

    SessionService service = new(
      userService,
      GetJwtTokenService()
    );
    
    // SessionToken badToken = new SessionToken("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI2M2Q4ZTM5NDc4NzY2NDAxNTAzMjZjM2IiLCJuYmYiOjE2NzUxNTg0NzUsImV4cCI6MTY3NTE1ODUzNSwiaWF0IjoxNjc1MTU4NDc1LCJpc3MiOiJiaXJjaGVnYW1lcy5jb20iLCJhdWQiOiJiaXJjaGVnYW1lcy5jb21URVNUIn0.CJbCUxDng92waygvQBx9mXGdKurVfljGevjg9Bfm06PUQMGcLPWbiYz9gWIMduiqalB4rd6mJp4EwaSHFgC-yQ");
    TokenWrapper badToken = new("bad_token");

    UserModel? user = await service.ValidateSessionToken(badToken);

    Assert.Null(user);
  }

  [Fact]
  public async Task ValidateSessionToken_Returns_User_With_Good_Token()
  {
    string username = "oldcheddar";
    string password = "password";

    UserService userService = await GetUserServiceWithUserAndPassword(username, password);

    SessionService service = new(
      userService,
      GetJwtTokenService()
    );

    TokenWrapper? goodToken = await service.GenerateSessionToken(
      new Credentials(
        username,
        password
      )
    );

    Assert.NotNull(goodToken);

    UserModel? user = await service.ValidateSessionToken(goodToken);
    Assert.NotNull(user);
    Assert.Equal(username, user.UserDetails.Username);
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