using System.Text.Json.Serialization;

namespace BircheMmoUserApi.Models;

///<summary>
/// <c>NewUserModel</c> describes a new User being supplied by a controller. It is missing most properties, like an Id, which will be created when adding to the database.
/// </summary>
public record NewUserModel
{
  public Credentials Credentials { get; set; }
  public string EmailAddress { get; set; }

  public NewUserModel(string username, string emailAddress, string password)
  {
    Credentials = new(
      username,
      password
    );
    EmailAddress = emailAddress;
  }
}