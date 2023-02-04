using System.Security.Claims;
using BircheMmoUserApi.Models;

namespace BircheMmoUserApi.Services;

public interface IJwtService
{
  public TokenWrapper GenerateToken(TokenData data);
  public TokenData? ValidateToken(TokenWrapper token);
}