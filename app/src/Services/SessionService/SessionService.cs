using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BircheMmoUserApi.Config;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

public class SessionService : ISessionService
{
  private readonly JwtConfig jwtConfig;
  private readonly JwtSecurityTokenHandler tokenHandler;
  private readonly IUserRepository userRepository;

  public SessionService(IUserRepository userRepository, JwtConfig jwtConfig)
  {
    this.userRepository = userRepository;
    this.jwtConfig = jwtConfig;
    tokenHandler = new();
  }

  public async Task<SessionToken?> GenerateSessionToken(Credentials credentials)
  {
    UserModel? user = await userRepository.FindUserByUsername(credentials.Username);
    if (user is null) return null;

    if (IsCredentialsForUserValid(credentials, user) == false) return null;

    SecurityToken token = tokenHandler.CreateToken(GetSecurityTokenDescriptor(user));
    return new SessionToken(
      tokenHandler.WriteToken(token)
    );
  }

  private bool IsCredentialsForUserValid(Credentials credentials, UserModel user)
  {
    return BCrypt.Net.BCrypt.Verify(credentials.Password, user.HashedPassword);
  }

  private SecurityTokenDescriptor GetSecurityTokenDescriptor(UserModel user)
  {
    Dictionary<string, object> claims = new();
    claims.Add("userId", user.Id);
    return new SecurityTokenDescriptor()
    {
      Expires = DateTime.UtcNow.Add(jwtConfig.Expires),
      Issuer = jwtConfig.Issuer,
      Audience = jwtConfig.Audience,
      SigningCredentials = new(
        new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Key)),
        SecurityAlgorithms.HmacSha512Signature
      ),
      Claims = claims
    };
  }

  public async Task<UserModel?> ValidateSessionToken(SessionToken token)
  {
    try
    {
      ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(
        token.Token,
        GetTokenValidationParameters(),
        out _
      );
      ObjectId id = ObjectId.Parse(claimsPrincipal.FindFirst("userId")?.Value);
      UserModel? user = await userRepository.FindUserById(id);
      return user;
    }
    catch
    {
      return null;
    }
  }

  private TokenValidationParameters GetTokenValidationParameters()
  {
    return new TokenValidationParameters()
    {
      ValidateIssuerSigningKey = true,
      IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(jwtConfig.Key)),
      ValidateIssuer = false,
      ValidateAudience = false,
      ClockSkew = TimeSpan.Zero
    };
  }
}