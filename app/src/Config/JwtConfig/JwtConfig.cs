namespace BircheMmoUserApi.Config;

public class JwtConfig
{
  public string Issuer { get; set; } = "";
  public string Audience { get; set; } = "";
  public string Key { get; set; } = Environment.GetEnvironmentVariable("ASPNETCORE_JWTCONFIG_KEY") ?? "";
  public TimeSpan Expires { get; set; } = TimeSpan.Zero;

  /// <summary>
  /// <c>JwtConfig</c> stores information needed by multiple services to generate and validate authentication tokens.
  /// Key defaults to a value determined by the proper environment variable if not set explicitly.
  /// </summary>
}