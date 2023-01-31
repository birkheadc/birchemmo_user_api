using System;
using BircheMmoUserApi.Config;

namespace BircheMmoUserApiTests.Mocks.Config;

public class MockJwtConfig_GoodData : JwtConfig
{
  public MockJwtConfig_GoodData()
  {
    Issuer = "birchegames.mock";
    Audience = "birchegames.mock";
    Key = "woeiruqfjsdov8sdf8igjnuq93u98uofb8isufasidfuia8wojfo3jfias";
    Expires = TimeSpan.FromMinutes(1);
  }
}