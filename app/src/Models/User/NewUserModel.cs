namespace BircheMmoUserApi.Models;

///<summary>
/// <c>NewUserModel</c> describes a new User being supplied by a controller. It is missing most properties, like an Id, which will be created when adding to the database.
/// </summary>
public record NewUserModel
{
  public string Username { get; set; } = "";
  public string Password { get; set; } = "";
  public Role Role { get; set; } = Role.USER;

  public NewUserModel(string username, string password, Role role)
  {
    Username = username;
    Password = password;
    Role = role;
  }
}