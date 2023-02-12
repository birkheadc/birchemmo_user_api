using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Services;

///<summary>
/// <c>IUserViewService</c> sterilizes UserModels into UserViewModels, making it more difficult to accidentally expose internal models to external users.
/// </summary>
public interface IUserViewService
{
  public Task<IEnumerable<UserViewModel>> GetAllUsers();
  public Task<UserViewModel?> GetUserByUsername(string username);
  public Task<UserViewModel?> GetUserById(string id);
  public Task<UserViewModel?> GetUserById(ObjectId id);
  public Task<UserViewModel?> CreateUser(NewUserModel user);
  public Task<UserViewModel?> CreateAdmin(NewUserModel user);
  public Task DeleteUserById(ObjectId id);
  public Task DeleteUserById(string id);
  public Task UpdateUserDetails(ObjectId id, UserDetails userDetails);
  public Task UpdatePassword(ObjectId id, string password);
  
}