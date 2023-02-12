using MongoDB.Bson;

namespace BircheMmoUserApi.Models;

///<summary>
/// <c>UserViewModel</c> is the version of the User model that should be returned when a User is requested from a controller.
/// </summary>
public record UserViewModel
{
  public UserDetails UserDetails { get; set; }
  public string Id { get; set; }

  public UserViewModel(string id, UserDetails userDetails)
  {
    UserDetails = userDetails;
    Id = id;
  }
}