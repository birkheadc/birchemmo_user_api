using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Repositories;

public interface IUserRepository
{
  public Task<IEnumerable<UserModel>> FindAllUsers();
  public Task<UserModel?> FindUserByUsername(string username);
  public Task<UserModel?> FindUserById(ObjectId id);
  public Task<UserModel?> FindUserByEmailAddress(string emailAddress);
  public Task<UserModel?> CreateUser(UserModel user);
  public Task DeleteUserById(ObjectId id);
  public Task UpdateUserDetails(ObjectId id, UserDetails userDetails);
  public Task UpdatePassword(ObjectId id, string newPassword);
}