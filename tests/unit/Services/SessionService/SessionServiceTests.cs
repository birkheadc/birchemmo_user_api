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


  }

  private async Task<InMemoryUserRepository> GetInMemoryUserRepositoryWithUserAndPassword(string username, string password)
  {

    InMemoryUserRepository repository = new();

    await repository.CreateUser(new UserModel(
      ObjectId.GenerateNewId(),
      username,
      BCrypt.Net.BCrypt.HashPassword(password),
      Role.USER,
      false
    ));

    return repository;
  }
}