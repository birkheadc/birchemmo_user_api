using MongoDB.Bson;

namespace BircheMmoUserApi.Models;

public record UserModel
{
  public ObjectId Id { get; set; }
  public string Username { get; set; } = "";
  public string HashedPassword { get; set; } = "";
  public Role Role { get; set; } = Role.USER;
  public bool IsEmailVerified { get; set; }
}