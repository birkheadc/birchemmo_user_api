namespace BircheMmoUserApiUnitTests.Mocks.Repositories;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApiUnitTests.Mocks.Builders;
using MongoDB.Bson;

#pragma warning disable 1998
public class MockUserRepository_ReturnsGoodData : IUserRepository
{
  private List<UserModel> users = new();

  public MockUserRepository_ReturnsGoodData()
  {
    users.Add(
      new MockUserModelBuilder()
        .WithRole(Role.ADMIN)
        .WithUsername("admin")
        .Build()
    );
    users.Add(
      new MockUserModelBuilder()
        .WithRole(Role.UNVALIDATED_USER)
        .WithUsername("unvalidated_user")
        .Build()
    );
    users.Add(
      new MockUserModelBuilder()
        .WithRole(Role.VALIDATED_USER)
        .WithUsername("validated_user")
        .Build()
    );
    users.Add(
      new MockUserModelBuilder()
        .WithRole(Role.SUPER_ADMIN)
        .WithUsername("super_admin")
        .Build()
    );
    users.Add(
      new MockUserModelBuilder()
        .WithRole(Role.ADMIN)
        .WithUsername("admin")
        .Build()
    );
  }
  public Task<UserModel?> CreateUser(UserModel user)
  {
    throw new System.NotImplementedException();
  }

  public Task DeleteUserById(ObjectId id)
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

  public Task UpdateUserDetails(ObjectId id, UserDetails userDetails)
  {
    throw new NotImplementedException();
  }

  public Task UpdatePassword(ObjectId id, string newPassword)
  {
    throw new NotImplementedException();
  }
}