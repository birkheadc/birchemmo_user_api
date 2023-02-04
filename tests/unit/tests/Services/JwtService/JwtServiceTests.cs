using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using BircheMmoUserApiUnitTests.Mocks.Config;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiUnitTests.Services;

public class JwtServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    JwtService service = new(new MockJwtConfig_GoodData());

    Assert.NotNull(service);
  }

  [Fact]
  public void GenerateToken_Returns_Token()
  {
    JwtService service = new(new MockJwtConfig_GoodData());

    TokenWrapper token = service.GenerateToken(
      new TokenData(
        ObjectId.Empty,
        TokenType.Login
      )
    );

    Assert.NotNull(token);
  }

  [Theory]
  [InlineData(TokenType.Null)]
  [InlineData(TokenType.Login)]
  [InlineData(TokenType.EmailVerification)]
  public void ValidateToken_Correctly_Validates_Token(TokenType tokenType)
  {
    ObjectId id = ObjectId.GenerateNewId();
    JwtService service = new(new MockJwtConfig_GoodData());

    TokenWrapper token = service.GenerateToken(
      new TokenData(
        id,
        tokenType
      )
    );

    TokenData? data = service.ValidateToken(token);

    Assert.NotNull(data);

    Assert.Equal(id, data.UserId);
    Assert.Equal(tokenType, data.TokenType);
  }
}