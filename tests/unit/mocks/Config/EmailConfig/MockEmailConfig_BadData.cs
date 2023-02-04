using System;
using BircheMmoUserApi.Config;

namespace BircheMmoUserApiUnitTests.Mocks.Config;

public class MockEmailConfig_BadData : EmailConfig
{
  public MockEmailConfig_BadData()
  {
    Name = "";
    Address = "";
    SmtpServer = "";
    Port = -1;
    Username = "";
    Password = "";
  }
}