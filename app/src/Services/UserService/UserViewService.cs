using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

public class UserViewService : IUserViewService
{
  private readonly IUserService userService;

  public UserViewService(IUserService userService)
  {
    this.userService = userService;
  }

  public async Task<UserViewModel?> CreateUser(NewUserModel user)
  {
    UserModel? userModel = await userService.CreateUser(user);
    return ToViewModel(userModel);
  }

  public async Task DeleteUserById(ObjectId id)
  {
    await userService.DeleteUserById(id);
  }

  public async Task UpdateUser(UserViewModel user)
  {
    await userService.UpdateUser(user);
  }

  public async Task<IEnumerable<UserViewModel>> GetAllUsers()
  {
    IEnumerable<UserModel> userModels = await userService.GetAllUsers();
    return ToViewModel(userModels);
  }

  public async Task<UserViewModel?> GetUserById(string id)
  {
    UserModel? userModel = await userService.GetUserById(ObjectId.Parse(id));
    return ToViewModel(userModel);
  }

  public async Task<UserViewModel?> GetUserByUsername(string username)
  {
    UserModel? userModel = await userService.GetUserByUsername(username);
    return ToViewModel(userModel);
  }

  private UserViewModel? ToViewModel(UserModel? userModel)
  {
    if (userModel is null) return null;
    return new UserViewModel
    (
      userModel.Id.ToString(),
      userModel.UserDetails.Username,
      userModel.UserDetails.EmailAddress,
      userModel.UserDetails.Role,
      userModel.IsEmailVerified
    );
  }

  private IEnumerable<UserViewModel> ToViewModel(IEnumerable<UserModel> userModels)
  {
    List<UserViewModel> userViewModels = new();
    foreach (UserModel userModel in userModels)
    {
      userViewModels.Add(ToViewModel(userModel)!);
    }
    return userViewModels;
  }

  public async Task UpdateUser(UserModel user)
  {
    await userService.UpdateUser(user);
  }
}