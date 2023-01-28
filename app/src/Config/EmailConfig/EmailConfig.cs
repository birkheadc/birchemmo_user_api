namespace BircheMmoUserApi.Config;

public class EmailConfig
{
  public string Name { get; set; }
  public string Address { get; set; }
  public string SmtpServer { get; set; }
  public int Port { get; set; }
  public string Username { get; set; }
  public string Password { get; set; }

  /// <summary>
  /// <c>EmailConfig</c> stores information needed by EmailService to login to the application's email account.
  /// The default constructor builds itself with data from environment variables.
  /// </summary>
  public EmailConfig()
  {
    Name = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_NAME") ?? "";
    Address = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_NAME") ?? "";
    SmtpServer = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_NAME") ?? "";
    Port = Int16.Parse(Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_NAME") ?? "0");
    Username = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_NAME") ?? "";
    Password = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_NAME") ?? "";
  }
}