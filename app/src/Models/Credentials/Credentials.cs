namespace BircheMmoUserApi.Models;

public record Credentials
{
  public string Username { get; set; } = "";
  public string Password { get; set; } = "";

  public Credentials()
  {
    
  }

  public Credentials(string username, string password)
  {
    Username = username;
    Password = password;
  }
}