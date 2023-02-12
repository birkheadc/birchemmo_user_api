using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

public class UserViewService : IUserViewService
{
  private readonly IUserService userService;
  private readonly UserConverter converter;

  public UserViewService(IUserService userService)
  {
    this.userService = userService;
    converter = new();
  }

  public async Task<UserViewModel?> CreateUser(NewUserModel user)
  {
    UserModel? userModel = await userService.CreateUser(user);
    return ToViewModel(userModel);
  }

  public async Task<UserViewModel?> CreateAdmin(NewUserModel user)
  {
    UserModel? userModel = await userService.CreateUser(user, Role.ADMIN);
    return ToViewModel(userModel);
  }

  public async Task DeleteUserById(string id)
  {
    await DeleteUserById(ObjectId.Parse(id));
  }

  public async Task DeleteUserById(ObjectId id)
  {
    await userService.DeleteUserById(id);
  }

  public async Task<IEnumerable<UserViewModel>> GetAllUsers()
  {
    IEnumerable<UserModel> userModels = await userService.GetAllUsers();
    return ToViewModel(userModels);
  }

  public async Task<UserViewModel?> GetUserById(string id)
  {
    return await GetUserById(ObjectId.Parse(id));
  }

  public async Task<UserViewModel?> GetUserById(ObjectId id)
  {
    UserModel? userModel = await userService.GetUserById(id);
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
    return converter.ToUserViewModel(userModel);
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

  public async Task UpdateUserDetails(ObjectId id, UserDetails userDetails)
  {
    await userService.UpdateUserDetails(id, userDetails);
  }

  public async Task UpdatePassword(ObjectId id, string password)
  {
    await userService.UpdatePassword(id, password);
  }
}