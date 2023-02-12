using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApiUnitTests.Mocks.Builders;

/// <summary>
/// <c>MockUserModelBuilder</c> helps quickly build mock users for seeding mock service / repositories.
/// If Build is called directly after instantiating, a user with default values is returned. (Values that must be unique are randomized)
/// With[...] functions allow changing only values that matter for the particular test case.
/// </summary>
public class MockUserModelBuilder
{
  private UserModel userModel;

  public MockUserModelBuilder()
  {
    ObjectId id = ObjectId.GenerateNewId();
    string username = "user_" + id.ToString();
    userModel = new(
      id,
      BCrypt.Net.BCrypt.HashPassword("password"),
      new UserDetails(
        username,
        username + "@site.com",
        Role.VISITOR,
        false
      )
    );
  }

  public UserModel Build()
  {
    return userModel;
  }

  public MockUserModelBuilder WithRole(Role role)
  {
    userModel.UserDetails.Role = role;
    return this;
  }

  public MockUserModelBuilder WithUsername(string username)
  {
    userModel.UserDetails.Username = username;
    return this;
  }

  public MockUserModelBuilder WithPassword(string password)
  {
    userModel.HashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
    return this;
  }

  public MockUserModelBuilder WithEmailAddress(string emailAddress)
  {
    userModel.UserDetails.EmailAddress = emailAddress;
    return this;
  }
}