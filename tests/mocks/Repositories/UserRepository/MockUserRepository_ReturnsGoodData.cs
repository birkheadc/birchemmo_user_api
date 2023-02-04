namespace BircheMmoUserApiTests.Mocks.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using MongoDB.Bson;

#pragma warning disable 1998
public class MockUserRepository_ReturnsGoodData : IUserRepository
{
  private List<UserModel> users = new();

  public MockUserRepository_ReturnsGoodData()
  {
    users.Add(new UserModel(
      ObjectId.GenerateNewId(),
      "user_1",
      BCrypt.Net.BCrypt.HashPassword("hashasdfasdfedpassword"),
      "test@test.com",
      Role.ADMIN,
      true
    ));
    users.Add(new UserModel(
      ObjectId.GenerateNewId(),
      "user_2",
      BCrypt.Net.BCrypt.HashPassword("hashedpassasdfasdword"),
      "example@example.com",
      Role.UNVALIDATED_USER,
      true
    ));
    users.Add(new UserModel(
      ObjectId.GenerateNewId(),
      "user_3",
      BCrypt.Net.BCrypt.HashPassword("hasheasdfasddpassword"),
      "user@place.extension",
      Role.UNVALIDATED_USER,
      true
    ));
    users.Add(new UserModel(
      ObjectId.GenerateNewId(),
      "user_4",
      BCrypt.Net.BCrypt.HashPassword("hashedpasasdfasdsword"),
      "user_4@my.site",
      Role.UNVALIDATED_USER,
      false
    ));
  }
  public Task<UserModel?> CreateUser(UserModel user)
  {
    throw new System.NotImplementedException();
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
    return users;
  }

  public async Task<UserModel?> FindUserById(ObjectId id)
  {
    try
    {
      return users.Find(u => u.Id == id);
    }
    catch (ArgumentNullException)
    {
      return null;
    }
  }

  public async Task<UserModel?> FindUserByUsername(string username)
  {
    try
    {
      return users.Find(u => u.UserDetails.Username == username);
    }
    catch (ArgumentNullException)
    {
      return null;
    }
  }

  public Task UpdateUser(UserModel user)
  {
    throw new NotImplementedException();
  }
}