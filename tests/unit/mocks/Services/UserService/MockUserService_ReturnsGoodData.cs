using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;
using BircheMmoUserApiUnitTests.Mocks.Builders;
using BircheMmoUserApiUnitTests.Mocks.Repositories;
using MongoDB.Bson;

namespace BircheMmoUserApiUnitTests.Mocks.Services;

public class MockUserService_ReturnsGoodData : IUserService
{
  private readonly IUserRepository repository = new MockUserRepository_ReturnsGoodData();
  public async Task<UserModel?> CreateUser(NewUserModel user, Role role = Role.UNVALIDATED_USER)
  {
    UserModel userModel = new MockUserModelBuilder()
      .WithRole(role)
      .WithUsername(user.Credentials.Username)
      .WithPassword(user.Credentials.Password)
      .WithEmailAddress(user.EmailAddress)
      .Build();

    return await repository.CreateUser(userModel);
  }

  public async Task DeleteUserById(ObjectId id)
  {
    await repository.DeleteUserById(id);
  }

  public async Task<IEnumerable<UserModel>> GetAllUsers()
  {
    return await repository.FindAllUsers();
  }

  public async Task<UserModel?> GetUserById(ObjectId id)
  {
    return await repository.FindUserById(id);
  }

  public async Task<UserModel?> GetUserByUsername(string username)
  {
    return await repository.FindUserByUsername(username);
  }

  private string HashPassword(string password)
  {
    return BCrypt.Net.BCrypt.HashPassword(password);
  }

  public Task UpdateUser(UserModel user)
  {
    throw new System.NotImplementedException();
  }

  public Task UpdateUserDetails(ObjectId id, UserDetails userDetails)
  {
    throw new System.NotImplementedException();
  }

  public Task UpdatePassword(ObjectId id, string password)
  {
    throw new System.NotImplementedException();
  }
}