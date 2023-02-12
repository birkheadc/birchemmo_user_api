using BircheMmoUserApi.Config;

namespace BircheMmoUserApiUnitTests.Mocks.Config;

public class MockEmailVerificationConfig_ReturnsGoodData : EmailVerificationConfig
{
  public MockEmailVerificationConfig_ReturnsGoodData()
  {
    FrontEndUrl = "http://localhost:3000";
    HtmlTemplatePath = "./assets/EmailVerificationTemplate/EmailVerificationTemplate.html";
  }
}