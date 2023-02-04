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
      userModel.UserDetails.Role,
      userModel.IsEmailVerified
    );
  }

  public UserModel ToUserModel(NewUserModel newUserModel, string hashedPassword)
  {
    UserModel userModel = new(
      ObjectId.GenerateNewId(),
      newUserModel.UserDetails.Username,
      hashedPassword,
      newUserModel.UserDetails.EmailAddress,
      newUserModel.UserDetails.Role,
      false
    );

    return userModel;
  }
}