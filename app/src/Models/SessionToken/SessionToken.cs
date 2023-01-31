namespace BircheMmoUserApi.Models;

public record SessionToken
{
  public string Token { get; set; } = "";
  public SessionToken(string token)
  {
    Token = token;
  }
}