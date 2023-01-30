namespace BircheMmoUserApi.Config;

public class EmailConfig
{
  public string Name { get; set; } = "";
  public string Address { get; set; } = "";
  public string SmtpServer { get; set; } = "";
  public int Port { get; set; } = 0;
  public string Username { get; set; } = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_USERNAME") ?? "";
  public string Password { get; set; } = Environment.GetEnvironmentVariable("ASPNETCORE_EMAILCONFIG_PASSWORD") ?? "";

  /// <summary>
  /// <c>EmailConfig</c> stores information needed by EmailService to login to the application's email account.
  /// Username and Password default to values determined by the proper environment variables if not set explicitly.
  /// </summary>
}