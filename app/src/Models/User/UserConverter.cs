using MongoDB.Bson;

namespace BircheMmoUserApi.Models;

public class UserConverter
{
  public UserViewModel ToUserViewModel(UserModel userModel)
  {
    return new UserViewModel
    (
      userModel.Id.ToString(),
      userModel.UserDetails
    );
  }

  public UserModel ToUserModel(NewUserModel newUserModel, Role role)
  {
    UserModel userModel = new(
      ObjectId.GenerateNewId(),
      BCrypt.Net.BCrypt.HashPassword(newUserModel.Credentials.Password),
      new UserDetails(
        newUserModel.Credentials.Username,
        newUserModel.EmailAddress,
        role,
        newUserModel.SendMeUpdates
      )
    );

    return userModel;
  }
}