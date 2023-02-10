using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BircheMmoUserApi.Config;
using BircheMmoUserApi.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

public class SessionService : ISessionService
{
  private readonly IUserService userService;
  private readonly IJwtService jwtTokenService;

  public SessionService(IUserService userService, IJwtService jwtTokenService)
  {
    this.userService = userService;
    this.jwtTokenService = jwtTokenService;
  }

  public async Task<TokenWrapper?> GenerateSessionToken(Credentials credentials)
  {
    UserModel? user = await userService.GetUserByUsername(credentials.Username);
    if (user is null) return null;

    if (IsCredentialsForUserValid(credentials, user) == false) return null;

    return jwtTokenService.GenerateToken(
      new TokenData(
        user.Id,
        TokenType.Login
      ),
      TimeSpan.FromMinutes(1)
    );
  }

  private bool IsCredentialsForUserValid(Credentials credentials, UserModel user)
  {
    return BCrypt.Net.BCrypt.Verify(credentials.Password, user.HashedPassword);
  }

  public async Task<UserModel?> ValidateSessionToken(TokenWrapper token)
  {
    TokenData? data = jwtTokenService.ValidateToken(token);
    if (data is null) return null;
    if (data.TokenType != TokenType.Login) return null;
    return await userService.GetUserById(data.UserId);
  }
}