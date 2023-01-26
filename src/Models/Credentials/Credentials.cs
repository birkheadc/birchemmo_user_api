namespace BircheMmoUserApi.Models;

public record Credentials
{
  public string Username { get; set; } = "";
  public string Password { get; set; } = "";
}