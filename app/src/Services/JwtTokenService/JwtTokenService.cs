using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BircheMmoUserApi.Config;
using BircheMmoUserApi.Models;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

public class JwtTokenService : IJwtTokenService
{
  private readonly JwtConfig jwtConfig;
  private readonly JwtSecurityTokenHandler tokenHandler;

  public JwtTokenService(JwtConfig jwtConfig)
  {
    this.jwtConfig = jwtConfig;
    tokenHandler = new();
  }

  public TokenWrapper GenerateToken(TokenData data)
  {
    SecurityToken token = tokenHandler.CreateToken(GetSecurityTokenDescriptor(data));
    return new TokenWrapper(
      tokenHandler.WriteToken(token)
    );
  }

  private SecurityTokenDescriptor GetSecurityTokenDescriptor(TokenData data)
  {
    Dictionary<string, object> claims = new();
    claims.Add("userId", data.UserId);
    claims.Add("tokenType", data.TokenType);
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

  public TokenData? ValidateToken(TokenWrapper token)
  {
    try
    {
      ClaimsPrincipal claimsPrincipal = tokenHandler.ValidateToken(
        token.Token,
        GetTokenValidationParameters(),
        out _
      );

      TokenType tokenType = (TokenType)Enum.Parse(typeof(TokenType), claimsPrincipal.FindFirst("tokenType")?.Value ?? "Null");
      ObjectId id = ObjectId.Parse(claimsPrincipal.FindFirst("userId")?.Value);

      return new TokenData(
        id,
        tokenType
      );
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