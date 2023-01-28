using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

///<summary>
/// <c>IUserService</c> provides access to UserModels. It should not be directly accessed by a controller as it exposes users' sensitive data.
/// </summary>
public interface IUserService
{
  public Task<IEnumerable<UserModel>> GetAllUsers();
  public Task<UserModel?> GetUserByUsername(string username);
  public Task<UserModel?> GetUserById(ObjectId id);
  public Task<UserModel?> CreateUser(NewUserModel user);
  public Task DeleteUserById(ObjectId id);
  public Task EditUser(UserViewModel user);
}