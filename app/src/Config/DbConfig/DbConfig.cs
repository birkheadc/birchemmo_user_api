namespace BircheMmoUserApi.Config;

public class DbConfig
{
  public string ConnectionString { get; set; } = Environment.GetEnvironmentVariable("ASPNETCORE_DBCONFIG_CONNECTION_STRING") ?? "";
  public string DatabaseName { get; set; } = "";
  public string UserCollectionName { get; set; } = "";
  /// <summary>
  /// <c>DbConfig</c> stores information needed by UserRepository to connect to the application's database.
  /// </summary>
}