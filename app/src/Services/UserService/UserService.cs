using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

public class UserService : IUserService
{
  private readonly IUserRepository userRepository;

  public UserService(IUserRepository userRepository)
  {
    this.userRepository = userRepository;
  }

  public async Task<UserModel?> CreateUser(NewUserModel user)
  {
    UserModel? userModel = await userRepository.CreateUser(user);
    return userModel;
  }

  public async Task DeleteUserById(ObjectId id)
  {
    await userRepository.DeleteUserById(id);
  }

  public async Task EditUser(UserViewModel user)
  {
    await userRepository.EditUser(user);
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
}