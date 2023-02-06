namespace BircheMmoUserApiUnitTests.Mocks.Repositories;

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
      "admin",
      BCrypt.Net.BCrypt.HashPassword("password"),
      "test@test.com",
      Role.ADMIN,
      true
    ));
    users.Add(new UserModel(
      ObjectId.GenerateNewId(),
      "unvalidated_user",
      BCrypt.Net.BCrypt.HashPassword("password"),
      "example@example.com",
      Role.UNVALIDATED_USER,
      true
    ));
    users.Add(new UserModel(
      ObjectId.GenerateNewId(),
      "validated_user",
      BCrypt.Net.BCrypt.HashPassword("password"),
      "user@place.extension",
      Role.VALIDATED_USER,
      true
    ));
    users.Add(new UserModel(
      ObjectId.GenerateNewId(),
      "super_admin",
      BCrypt.Net.BCrypt.HashPassword("password"),
      "user_4@my.site",
      Role.SUPER_ADMIN,
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