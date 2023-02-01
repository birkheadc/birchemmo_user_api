namespace BircheMmoUserApi.Models;

/// <summary>
/// <c>UserDetails</c> contains details about the user that should be included in every model of the user, incoming, outgoing, and internal.
/// </summary>
public record UserDetails
{
  public string Username { get; set; }
  public string EmailAddress { get; set; }
  public Role Role { get; set; }
  public UserDetails(string username, string emailAddress, Role role)
  {
    Username = username;
    EmailAddress = emailAddress;
    Role = role;
  }
}