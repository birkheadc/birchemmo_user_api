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
    UserModel userModel = ToUserModel(user);
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

  private UserModel ToUserModel(NewUserModel newUserModel)
  {
    UserModel userModel = new(
      ObjectId.GenerateNewId(),
      newUserModel.UserDetails.Username,
      HashPassword(newUserModel.Password),
      newUserModel.UserDetails.EmailAddress,
      newUserModel.UserDetails.Role,
      false
    );

    return userModel;
  }

  private string HashPassword(string password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password);
  }

  public async Task UpdateUser(UserModel user)
  {
    await userRepository.UpdateUser(user);
  }
}