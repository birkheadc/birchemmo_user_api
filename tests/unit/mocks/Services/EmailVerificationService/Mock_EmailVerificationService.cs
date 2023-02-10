using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;

namespace BircheMmoUserApiUnitTests.Mocks.Services;

public class MockEmailVerificationService : IEmailVerificationService
{
  public Task<TokenWrapper?> GenerateForUser(UserModel user)
  {
    throw new System.NotImplementedException();
  }

  public Task<bool> ValidateUser(TokenWrapper token)
  {
    throw new System.NotImplementedException();
  }
}