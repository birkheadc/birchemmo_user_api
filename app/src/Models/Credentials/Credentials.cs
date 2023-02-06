using System.Text;

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

  public string ToBasicAuth()
  {
    return System.Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes(Username + ":" + Password));
  }
}