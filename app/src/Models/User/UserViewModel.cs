using MongoDB.Bson;

namespace BircheMmoUserApi.Models;

///<summary>
/// <c>UserViewModel</c> is the version of the User model that should be returned when a User is requested from a controller.
/// </summary>
public record UserViewModel
{
  public ObjectId Id { get; set; }
  public string Username { get; set; } = "";
  public Role Role { get; set; } = Role.USER;
  public bool IsEmailVerified { get; set; }

  public UserViewModel(ObjectId id, string username, Role role, bool isEmailVerified)
  {
    Id = id;
    Username = username;
    Role = role;
    IsEmailVerified = isEmailVerified;
  }
}