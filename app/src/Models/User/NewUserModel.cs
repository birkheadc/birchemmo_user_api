namespace BircheMmoUserApi.Models;

///<summary>
/// <c>NewUserModel</c> describes a new User being supplied by a controller. It is missing most properties, like an Id, which will be created when adding to the database.
/// </summary>
public record NewUserModel
{
  public UserDetails UserDetails { get; set; }
  public string Password { get; set; }

  public NewUserModel(string username, string password, string emailAddress, Role role)
  {
    UserDetails = new(
      username,
      emailAddress,
      role
    );
    Password = password;
  }
}