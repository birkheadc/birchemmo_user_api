namespace BircheMmoUserApi.Models;

public record TokenWrapper
{
  public string Token { get; set; } = "";
  public TokenWrapper(string token)
  {
    Token = token;
  }
}