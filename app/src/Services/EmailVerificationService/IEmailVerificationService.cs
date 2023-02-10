using BircheMmoUserApi.Models;

namespace BircheMmoUserApi.Services;

public interface IEmailVerificationService
{
  public Task<TokenWrapper?> GenerateForUser(UserModel user);
  public Task<bool> ValidateUser(TokenWrapper token);
}