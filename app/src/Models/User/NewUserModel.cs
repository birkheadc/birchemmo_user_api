using System.Text.Json.Serialization;

namespace BircheMmoUserApi.Models;

///<summary>
/// <c>NewUserModel</c> describes a new User being supplied by a controller. It is missing most properties, like an Id, which will be created when adding to the database.
/// </summary>
public record NewUserModel
{
  public UserDetails UserDetails { get; set; }
  public string Password { get; set; }

  // public NewUserModel()
  // {
  //   UserDetails = new(
  //     "",
  //     "",
  //     Role.USER
  //   );
  //   Password = "";
  // }
  public NewUserModel(UserDetails userDetails, string password)
  {
    UserDetails = userDetails;
    Password = password;
  }
}