using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using MongoDB.Bson;

namespace BircheMmoUserApiTests.Mocks.Services;

#pragma warning disable 1998
public class MockUserService_ReturnsNull : IUserService
{
  public async Task<UserModel?> CreateUser(NewUserModel user)
  {
    return null;
  }

  public async Task DeleteUserById(ObjectId id)
  {
    
  }

  public async Task EditUser(UserViewModel user)
  {

  }

  public async Task<IEnumerable<UserModel>> GetAllUsers()
  {
    return new List<UserModel>();
  }

  public async Task<UserModel?> GetUserById(ObjectId id)
  {
    return null;
  }

  public async Task<UserModel?> GetUserByUsername(string username)
  {
    return null;
  }
}