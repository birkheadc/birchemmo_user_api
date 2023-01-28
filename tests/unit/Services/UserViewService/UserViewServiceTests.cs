using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Services;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiTests.Services;

public class UserViewServiceTests
{
  [Fact]
  public void Service_Resolves()
  {
    UserViewService service = new(new MockUserService_ReturnsGoodData());

    Assert.NotNull(service);
  }

  [Fact]
  public async Task GetAllUsers_Returns_Empty_When_No_Users_Exist()
  {
    UserViewService service = new(new MockUserService_ReturnsNull());

    IEnumerable<UserViewModel> users = await service.GetAllUsers();

    Assert.Empty(users);
  }

  [Fact]
  public async Task GetAllUsers_Returns_Non_Empty_When_Users_Exist()
  {
    UserViewService uservice = new(new MockUserService_ReturnsGoodData());

    IEnumerable<UserViewModel> users = await uservice.GetAllUsers();

    Assert.NotEmpty(users);
  }

  [Fact]
  public async Task GetUserById_Returns_Null_When_Bad_Id()
  {
    UserViewService service = new(new MockUserService_ReturnsGoodData());

    UserViewModel? user = await service.GetUserById(ObjectId.Empty);

    Assert.Null(user);
  }

  [Fact]
  public async Task GetUserByUsername_Returns_Null_When_Bad_Username()
  {
    UserViewService service = new(new MockUserService_ReturnsGoodData());

    UserViewModel? user = await service.GetUserByUsername("this_user_does_not_exist");

    Assert.Null(user);
  }

  [Fact]
  public async Task GetUserById_Returns_User_When_Good_Id()
  {
    UserViewService service = new(new MockUserService_ReturnsGoodData());

    List<UserViewModel> users = new();
    users.AddRange(await service.GetAllUsers());

    UserViewModel? user = await service.GetUserById(users[0].Id);

    Assert.NotNull(user);
    Assert.True(user is not null && users[0].Id == user.Id);
  }

  [Fact]
  public async Task GetUserByUsername_Returns_User_When_Good_Username()
  {
    UserViewService service = new(new MockUserService_ReturnsGoodData());

    List<UserViewModel> users = new();
    users.AddRange(await service.GetAllUsers());

    UserViewModel? user = await service.GetUserByUsername(users[0].Username);

    Assert.NotNull(user);
    Assert.True(user is not null && users[0].Id == user.Id);
  }
}