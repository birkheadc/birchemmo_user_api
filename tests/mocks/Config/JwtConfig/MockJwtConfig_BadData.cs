using System;
using BircheMmoUserApi.Config;

namespace BircheMmoUserApiTests.Mocks.Config;

public class MockJwtConfig_BadData : JwtConfig
{
  public MockJwtConfig_BadData()
  {
    Issuer = "birchegames.mock";
    Audience = "birchegames.mock";
    Key = "";
    Expires = TimeSpan.Zero;
  }
}