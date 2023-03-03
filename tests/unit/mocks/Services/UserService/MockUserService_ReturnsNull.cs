using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using MongoDB.Bson;

namespace BircheMmoUserApiUnitTests.Mocks.Services;

#pragma warning disable 1998
public class MockUserService_ReturnsNull : IUserService
{
  public async Task<UserModel?> CreateUser(NewUserModel user, Role role = Role.UNVALIDATED_USER)
  {
    return null;
  }

  public async Task DeleteUserById(ObjectId id)
  {
    
  }

  public async Task<IEnumerable<UserModel>> GetAllUsers()
  {
    return new List<UserModel>();
  }

  public Task<UserModel?> GetUserByEmailAddress(string emailAddress)
  {
    throw new System.NotImplementedException();
  }

  public async Task<UserModel?> GetUserById(ObjectId id)
  {
    return null;
  }

  public async Task<UserModel?> GetUserByUsername(string username)
  {
    return null;
  }

  public Task UpdatePassword(ObjectId id, string password)
  {
    throw new System.NotImplementedException();
  }

  public Task UpdateUserDetails(ObjectId id, UserDetails userDetails)
  {
    throw new System.NotImplementedException();
  }
}