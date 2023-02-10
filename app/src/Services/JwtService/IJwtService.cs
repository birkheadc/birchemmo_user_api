using System.Security.Claims;
using BircheMmoUserApi.Models;

namespace BircheMmoUserApi.Services;

public interface IJwtService
{
  public TokenWrapper GenerateToken(TokenData data, TimeSpan expires);
  public TokenData? ValidateToken(TokenWrapper token);
}