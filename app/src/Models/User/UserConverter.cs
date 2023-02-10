using MongoDB.Bson;

namespace BircheMmoUserApi.Models;

public class UserConverter
{
  public UserViewModel ToUserViewModel(UserModel userModel)
  {
    return new UserViewModel
    (
      userModel.Id.ToString(),
      userModel.UserDetails.Username,
      userModel.UserDetails.EmailAddress,
      userModel.UserDetails.Role
    );
  }

  public UserModel ToUserModel(NewUserModel newUserModel)
  {
    UserModel userModel = new(
      ObjectId.GenerateNewId(),
      newUserModel.Credentials.Username,
      BCrypt.Net.BCrypt.HashPassword(newUserModel.Credentials.Password),
      newUserModel.EmailAddress,
      Role.UNVALIDATED_USER
    );

    return userModel;
  }
}