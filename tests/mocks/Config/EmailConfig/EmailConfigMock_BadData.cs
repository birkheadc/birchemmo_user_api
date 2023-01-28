using System;
using BircheMmoUserApi.Config;

namespace BircheMmoUserApiTests.Mocks.Config;

public class EmailConfigMock_BadData : EmailConfig
{
  public EmailConfigMock_BadData()
  {
    Name = "";
    Address = "";
    SmtpServer = "";
    Port = -1;
    Username = "";
    Password = "";
  }
}