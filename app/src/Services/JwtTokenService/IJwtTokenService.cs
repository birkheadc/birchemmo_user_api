using System.Security.Claims;
using BircheMmoUserApi.Models;

namespace BircheMmoUserApi.Services;

public interface IJwtTokenService
{
  public TokenWrapper GenerateToken(TokenData data);
  public TokenData? ValidateToken(TokenWrapper token);
}