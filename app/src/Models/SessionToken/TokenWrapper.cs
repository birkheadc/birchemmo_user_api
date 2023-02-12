namespace BircheMmoUserApi.Models;

public record TokenWrapper
{
  public string Token { get; set; } = "";
  public DateTime? Expires { get; set; }
  public TokenWrapper(string token, DateTime? expires = null)
  {
    Token = token;
    Expires = expires;
  }
}