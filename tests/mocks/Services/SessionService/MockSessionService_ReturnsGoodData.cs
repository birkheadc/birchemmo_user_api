using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;

namespace BircheMmoUserApiTests.Mocks.Services;

public class MockSessionService_ReturnsGoodData : ISessionService
{
  public Task<SessionToken?> GenerateSessionToken(Credentials credentials)
  {
    throw new System.NotImplementedException();
  }

  public Task<UserModel?> ValidateSessionToken(SessionToken token)
  {
    throw new System.NotImplementedException();
  }
}