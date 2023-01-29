using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Controllers;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiTests.Services;

public class UserControllerTests
{
  [Fact]
  public void Controller_Resolves()
  {
    UserController controller = new(GetUserViewService_ReturnsGoodData());

    Assert.NotNull(controller);
  }

  [Fact]
  public async Task GetAllUsers_Returns_Empty_List_When_No_Users()
  {
    UserController controller = new(GetUserViewService_ReturnsNull());

    OkObjectResult? result = (await controller.GetAllUsers()).Result as OkObjectResult;
    Assert.NotNull(result);

    IEnumerable<UserViewModel>? users = result.Value as IEnumerable<UserViewModel>;
    Assert.NotNull(users);
    Assert.Empty(users);
  }

  [Fact]
  public async Task GetAllUsers_Returns_List_When_Users_Exist()
  {
    UserController controller = new(GetUserViewService_ReturnsGoodData());

    OkObjectResult? result = (await controller.GetAllUsers()).Result as OkObjectResult;
    Assert.NotNull(result);

    IEnumerable<UserViewModel>? users = result.Value as IEnumerable<UserViewModel>;
    Assert.NotNull(users);
    Assert.NotEmpty(users);
  }

  [Fact]
  public async Task GetUserById_Returns_NotFound_When_Empty()
  {
    UserController controller = new(GetUserViewService_ReturnsNull());
    NotFoundResult? result = (await controller.GetUserById(ObjectId.Empty)).Result as NotFoundResult;
    Assert.NotNull(result);
  }

  [Fact]
  public async Task GetUserById_Returns_NotFound_When_Not_Exist()
  {
    UserController controller = new(GetUserViewService_ReturnsGoodData());
    NotFoundResult? result = (await controller.GetUserById(ObjectId.Empty)).Result as NotFoundResult;
    Assert.NotNull(result);
  }

  [Fact]
  public async Task GetUserById_Returns_User_When_Exist()
  {
    UserViewService service = (UserViewService)GetUserViewService_ReturnsGoodData();
    ObjectId id = ((List<UserViewModel>)await service.GetAllUsers())[0].Id;

    UserController controller = new(service);
    OkObjectResult? result = (await controller.GetUserById(id)).Result as OkObjectResult;
    Assert.NotNull(result);

    UserViewModel? user = result.Value as UserViewModel;
    Assert.NotNull(user);
  }

  private IUserViewService GetUserViewService_ReturnsGoodData()
  {
    return new UserViewService(new MockUserService_ReturnsGoodData());
  }

  private IUserViewService GetUserViewService_ReturnsNull()
  {
    return new UserViewService(new MockUserService_ReturnsNull());
  }
}