using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Repositories;

public interface IUserRepository
{
  public Task<IEnumerable<UserModel>> FindAllUsers();
  public Task<UserModel> FindUserByUsername(string username);
  public Task<UserModel> FindUserById(ObjectId id);
  public Task CreateUser(NewUserModel user);
  public Task DeleteUserById(ObjectId id);
  public Task EditUser(UserViewModel user);
}