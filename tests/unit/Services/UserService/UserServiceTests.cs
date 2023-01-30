namespace BircheMmoUserApiTests.Services;

using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Repositories;
using MongoDB.Bson;
using Xunit;

public class UserServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    UserService userService = new(new MockUserRepository_ReturnsGoodData());
    
    Assert.NotNull(userService);
  }

  [Fact]
  public async Task GetAllUsers_Returns_Empty_When_No_Users_Exist()
  {
    UserService userService = new(new MockUserRepository_ReturnsNull());

    IEnumerable<UserModel> users = await userService.GetAllUsers();

    Assert.Empty(users);
  }

  [Fact]
  public async Task GetAllUsers_Returns_Non_Empty_When_Users_Exist()
  {
    UserService userService = new(new MockUserRepository_ReturnsGoodData());

    IEnumerable<UserModel> users = await userService.GetAllUsers();

    Assert.NotEmpty(users);
  }

  [Fact]
  public async Task GetUserById_Returns_Null_When_Bad_Id()
  {
    UserService userService = new(new MockUserRepository_ReturnsGoodData());

    UserModel? user = await userService.GetUserById(ObjectId.Empty);

    Assert.Null(user);
  }

  [Fact]
  public async Task GetUserByUsername_Returns_Null_When_Bad_Username()
  {
    UserService userService = new(new MockUserRepository_ReturnsGoodData());

    UserModel? user = await userService.GetUserByUsername("this_user_does_not_exist");

    Assert.Null(user);
  }

  [Fact]
  public async Task GetUserById_Returns_User_When_Good_Id()
  {
    UserService userService = new(new MockUserRepository_ReturnsGoodData());

    List<UserModel> users = new();
    users.AddRange(await userService.GetAllUsers());

    UserModel? user = await userService.GetUserById(users[0].Id);

    Assert.NotNull(user);
    Assert.True(user is not null && users[0].Id == user.Id);
  }

  [Fact]
  public async Task GetUserByUsername_Returns_User_When_Good_Username()
  {
    UserService userService = new(new MockUserRepository_ReturnsGoodData());

    List<UserModel> users = new();
    users.AddRange(await userService.GetAllUsers());

    UserModel? user = await userService.GetUserByUsername(users[0].Username);

    Assert.NotNull(user);
    Assert.True(user is not null && users[0].Id == user.Id);
  }
}