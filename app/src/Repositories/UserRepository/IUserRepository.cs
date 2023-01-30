using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Repositories;

public interface IUserRepository
{
  public Task<IEnumerable<UserModel>> FindAllUsers();
  public Task<UserModel?> FindUserByUsername(string username);
  public Task<UserModel?> FindUserById(ObjectId id);
  public Task<UserModel?> CreateUser(NewUserModel newUser);
  public Task DeleteUserById(ObjectId id);
  public Task EditUser(UserViewModel user);
}