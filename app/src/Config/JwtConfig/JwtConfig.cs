namespace BircheMmoUserApi.Config;

public class JwtConfig
{
  public string Issuer { get; set; }
  public string Audience { get; set; }
  public string Key { get; set; }

  /// <summary>
  /// <c>JwtConfig</c> stores information needed by multiple services to generate and validate authentication tokens.
  /// The default constructor builds itself with data from environment variables.
  /// </summary>
  public JwtConfig()
  {
    Issuer = Environment.GetEnvironmentVariable("ASPNETCORE_JWTCONFIG_ISSUER") ?? "";
    Audience = Environment.GetEnvironmentVariable("ASPNETCORE_JWTCONFIG_AUDIENCE") ?? "";
    Key = Environment.GetEnvironmentVariable("ASPNETCORE_JWTCONFIG_KEY") ?? "";
  }
}