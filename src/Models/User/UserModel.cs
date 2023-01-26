using MongoDB.Bson;

namespace BircheMmoUserApi.Models;

/// <summary>
/// <c>UserModel</c> is the internal model describing a User.
/// It includes the user's hashed password, so should NEVER be returned by a controller, or otherwise let outside this application.
/// </summary>
public record UserModel
{
  public ObjectId Id { get; set; }
  public string Username { get; set; } = "";
  public string HashedPassword { get; set; } = "";
  public Role Role { get; set; } = Role.USER;
  public bool IsEmailVerified { get; set; }
}