using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

public interface ISessionService
{
  /// <summary>
  /// Returns a <c>SessionToken</c> if supplied with correct credentials, otherwise returns null.
  /// </summary>
  public Task<TokenWrapper?> GenerateSessionToken(Credentials credentials);
  /// <summary>
  /// Returns the <c>UserModel</c> that matches the token's user. Returns null if the token is not valid.
  /// </summary
  public Task<UserModel?> ValidateSessionToken(TokenWrapper token);
}