using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Repositories;
using MongoDB.Bson;

namespace BircheMmoUserApiTests.Mocks.Services;

public class MockUserService_ReturnsGoodData : IUserService
{
  private readonly IUserRepository repository = new MockUserRepository_ReturnsGoodData();
  public async Task<UserModel?> CreateUser(NewUserModel user)
  {
    UserModel userModel = new(
      ObjectId.GenerateNewId(),
      user.Username,
      HashPassword(user.Password),
      user.Role,
      false
    );
    return await repository.CreateUser(userModel);
  }

  public async Task DeleteUserById(ObjectId id)
  {
    await repository.DeleteUserById(id);
  }

  public async Task EditUser(UserViewModel user)
  {
    await repository.EditUser(user);
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
}