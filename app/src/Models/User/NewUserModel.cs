using System.Text.Json.Serialization;

namespace BircheMmoUserApi.Models;

///<summary>
/// <c>NewUserModel</c> describes a new User being supplied by a controller. It is missing most properties, like an Id, which will be created when adding to the database.
/// </summary>
public record NewUserModel
{
  public Credentials Credentials { get; set; }
  public string EmailAddress { get; set; }
  public bool SendMeUpdates { get; set; }

  public NewUserModel(Credentials credentials, string emailAddress, bool sendMeUpdates = false)
  {
    Credentials = credentials;
    EmailAddress = emailAddress;
    SendMeUpdates = sendMeUpdates;
  }
}