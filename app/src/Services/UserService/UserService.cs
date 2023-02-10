using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

public class UserService : IUserService
{
  private readonly IUserRepository userRepository;
  private readonly UserConverter converter;

  public UserService(IUserRepository userRepository)
  {
    this.userRepository = userRepository;
    converter = new();
  }

  public async Task<UserModel?> CreateUser(NewUserModel user, Role role = Role.UNVALIDATED_USER)
  {
    UserModel userModel = ToUserModel(user, role);
    UserModel? returnUser = await userRepository.CreateUser(userModel);
    return returnUser;
  }

  public async Task DeleteUserById(ObjectId id)
  {
    await userRepository.DeleteUserById(id);
  }

  public async Task UpdateUser(UserViewModel user)
  {
    await userRepository.UpdateUser(user);
  }

  public async Task UpdateUser(UserModel user)
  {
    await userRepository.UpdateUser(user);
  }

  public async Task<IEnumerable<UserModel>> GetAllUsers()
  {
    IEnumerable<UserModel> users = await userRepository.FindAllUsers();
    return users;
  }

  public async Task<UserModel?> GetUserById(ObjectId id)
  {
    UserModel? user = await userRepository.FindUserById(id);
    return user;
  }

  public async Task<UserModel?> GetUserByUsername(string username)
  {
    UserModel? user = await userRepository.FindUserByUsername(username);
    return user;
  }

  private UserModel ToUserModel(NewUserModel newUserModel, Role role)
  {
    UserModel userModel = converter.ToUserModel(newUserModel, role);
    return userModel;
  }
}