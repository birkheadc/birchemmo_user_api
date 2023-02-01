using BircheMmoUserApi.Models;

namespace BircheMmoUserApi.Services;

public interface IEmailVerificationService
{
  public Task<TokenWrapper?> GenerateEmailVerificationTokenForUser(UserModel user);
  public Task<bool> ValidateEmailVerificationTokenForUser(UserModel user, TokenWrapper token);
}