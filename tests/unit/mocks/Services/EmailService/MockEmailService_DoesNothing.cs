using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;

namespace BircheMmoUserApiUnitTests.Mocks.Services;

public class MockEmailService_DoesNothing : IEmailService
{
  public Task<bool> SendEmailAsync(string receiverName, string receiverAddress, string subject, string body)
  {
    throw new System.NotImplementedException();
  }

  public Task<bool> SendVerificationEmail(UserModel user)
  {
    throw new System.NotImplementedException();
  }
}