using MongoDB.Bson;

namespace BircheMmoUserApi.Models;

/// <summary>
/// <c>UserModel</c> is the internal model describing a User.
/// It includes the user's hashed password, so should NEVER be returned by a controller, or otherwise let outside this application.
/// </summary>
public record UserModel
{
  public UserDetails UserDetails { get; set; }
  public ObjectId Id { get; set; }
  public string HashedPassword { get; set; }
  public bool IsEmailVerified { get; set; }

  public UserModel(ObjectId id, string username, string hashedPassword, string emailAddress, Role role, bool isEmailVerified)
  {
    UserDetails = new(
      username,
      emailAddress,
      role
    );
    Id = id;
    HashedPassword = hashedPassword;
    IsEmailVerified = isEmailVerified;
  }
}