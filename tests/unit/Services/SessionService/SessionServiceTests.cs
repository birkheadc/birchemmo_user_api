using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Repositories;
using BircheMmoUserApiTests.Mocks.Config;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiTests.Services;

#pragma warning disable 1998
public class SessionServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    SessionService service = new(
      new MockUserRepository_ReturnsGoodData(),
      new MockJwtConfig_GoodData()
    );

    Assert.NotNull(service);
  }

  [Fact]
  public async Task GenerateSessionToken_Returns_Null_With_Bad_Credentials()
  {
    string username = "oldcheddar";
    string password = "password";

    InMemoryUserRepository repository = await GetInMemoryUserRepositoryWithUserAndPassword(username, password);

    Assert.NotNull(await repository.FindUserByUsername(username));

    SessionService service = new(
      repository,
      new MockJwtConfig_GoodData()
    );
    
    SessionToken? token = await service.GenerateSessionToken(new Credentials(
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

    InMemoryUserRepository repository = await GetInMemoryUserRepositoryWithUserAndPassword(username, password);

    Assert.NotNull(await repository.FindUserByUsername(username));

    SessionService service = new(
      repository,
      new MockJwtConfig_GoodData()
    );
    
    SessionToken? token = await service.GenerateSessionToken(new Credentials(
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

    InMemoryUserRepository repository = await GetInMemoryUserRepositoryWithUserAndPassword(username, password);

    SessionService service = new(
      repository,
      new MockJwtConfig_GoodData()
    );
    
    // SessionToken badToken = new SessionToken("eyJhbGciOiJIUzUxMiIsInR5cCI6IkpXVCJ9.eyJ1c2VySWQiOiI2M2Q4ZTM5NDc4NzY2NDAxNTAzMjZjM2IiLCJuYmYiOjE2NzUxNTg0NzUsImV4cCI6MTY3NTE1ODUzNSwiaWF0IjoxNjc1MTU4NDc1LCJpc3MiOiJiaXJjaGVnYW1lcy5jb20iLCJhdWQiOiJiaXJjaGVnYW1lcy5jb21URVNUIn0.CJbCUxDng92waygvQBx9mXGdKurVfljGevjg9Bfm06PUQMGcLPWbiYz9gWIMduiqalB4rd6mJp4EwaSHFgC-yQ");
    SessionToken badToken = new("bad_token");

    UserModel? user = await service.ValidateSessionToken(badToken);

    Assert.Null(user);
  }

  [Fact]
  public async Task ValidateSessionToken_Returns_User_With_Good_Token()
  {
    string username = "oldcheddar";
    string password = "password";

    InMemoryUserRepository repository = await GetInMemoryUserRepositoryWithUserAndPassword(username, password);

    SessionService service = new(
      repository,
      new MockJwtConfig_GoodData()
    );

    SessionToken? goodToken = await service.GenerateSessionToken(
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

  private async Task<InMemoryUserRepository> GetInMemoryUserRepositoryWithUserAndPassword(string username, string password)
  {

    InMemoryUserRepository repository = new();

    await repository.CreateUser(new UserModel(
      ObjectId.GenerateNewId(),
      username,
      BCrypt.Net.BCrypt.HashPassword(password),
      username + "@site.com",
      Role.USER,
      false
    ));

    return repository;
  }
}