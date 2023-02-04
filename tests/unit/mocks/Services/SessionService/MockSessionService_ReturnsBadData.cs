using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;

namespace BircheMmoUserApiUnitTests.Mocks.Services;

public class MockSessionService_ReturnsBadData : ISessionService
{
  public Task<TokenWrapper?> GenerateSessionToken(Credentials credentials)
  {
    throw new System.NotImplementedException();
  }

  public Task<UserModel?> ValidateSessionToken(TokenWrapper token)
  {
    throw new System.NotImplementedException();
  }
}