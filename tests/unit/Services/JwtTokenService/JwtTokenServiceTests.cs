using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Config;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiTests.Services;

public class JwtTokenServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    JwtTokenService service = new(new MockJwtConfig_GoodData());

    Assert.NotNull(service);
  }

  [Fact]
  public void GenerateToken_Returns_Token()
  {
    JwtTokenService service = new(new MockJwtConfig_GoodData());

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
    JwtTokenService service = new(new MockJwtConfig_GoodData());

    TokenWrapper token = service.GenerateToken(
      new TokenData(
        id,
        tokenType
      )
    );

    TokenData? data = service.ValidateToken(token);

    Assert.NotNull(data);

    Assert.Equal(data.UserId, id);
    Assert.Equal(data.TokenType, tokenType);
  }
}