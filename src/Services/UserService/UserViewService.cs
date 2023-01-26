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

  public async Task CreateUser(NewUserModel user)
  {
    await userService.CreateUser(user);
  }

  public async Task DeleteUserById(ObjectId id)
  {
    await userService.DeleteUserById(id);
  }

  public async Task EditUser(UserViewModel user)
  {
    await userService.EditUser(user);
  }

  public async Task<IEnumerable<UserViewModel>> GetAllUsers()
  {
    IEnumerable<UserModel> userModels = await userService.GetAllUsers();
    return ToViewModel(userModels);
  }

  public async Task<UserViewModel> GetUserById(ObjectId id)
  {
    UserModel userModel = await userService.GetUserById(id);
    return ToViewModel(userModel);
  }

  public async Task<UserViewModel> GetUserByUsername(string username)
  {
    UserModel userModel = await userService.GetUserByUsername(username);
    return ToViewModel(userModel);
  }

  private UserViewModel ToViewModel(UserModel userModel)
  {
    return new UserViewModel()
    {
      Id = userModel.Id,
      Username = userModel.Username,
      Role = userModel.Role,
      IsEmailVerified = userModel.IsEmailVerified
    };
  }

  private IEnumerable<UserViewModel> ToViewModel(IEnumerable<UserModel> userModels)
  {
    List<UserViewModel> userViewModels = new();
    foreach (UserModel userModel in userModels)
    {
      userViewModels.Add(ToViewModel(userModel));
    }
    return userViewModels;
  }
}