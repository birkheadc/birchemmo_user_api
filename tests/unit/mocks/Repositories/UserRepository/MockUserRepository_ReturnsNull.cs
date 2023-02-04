namespace BircheMmoUserApiUnitTests.Mocks.Repositories;


using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using MongoDB.Bson;

#pragma warning disable 1998
public class MockUserRepository_ReturnsNull : IUserRepository
{
  public async Task<UserModel?> CreateUser(UserModel user)
  {
    return null;
  }

  public Task DeleteUserById(ObjectId id)
  {
    throw new System.NotImplementedException();
  }

  public Task UpdateUser(UserViewModel user)
  {
    throw new System.NotImplementedException();
  }

  public async Task<IEnumerable<UserModel>> FindAllUsers()
  {
    return new List<UserModel>();
  }

  public async Task<UserModel?> FindUserById(ObjectId id)
  {
    return null;
  }

  public async Task<UserModel?> FindUserByUsername(string username)
  {
    return null;
  }

  public Task UpdateUser(UserModel user)
  {
    throw new System.NotImplementedException();
  }
}