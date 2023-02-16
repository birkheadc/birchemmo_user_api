using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Controllers;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using BircheMmoUserApiUnitTests.Mocks.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiUnitTests.Services;

public class UserControllerTests
{
  [Fact]
  public void Controller_Resolves()
  {
    UserController controller = new(GetUserService_ReturnsGoodData(), new MockEmailService_DoesNothing());

    Assert.NotNull(controller);
  }

  [Fact]
  public async Task GetAllUsers_Returns_Empty_List_When_No_Users()
  {
    UserController controller = new(GetUserService_ReturnsNull(), new MockEmailService_DoesNothing());

    OkObjectResult? result = (await controller.GetAllUsers()).Result as OkObjectResult;
    Assert.NotNull(result);

    IEnumerable<UserViewModel>? users = result.Value as IEnumerable<UserViewModel>;
    Assert.NotNull(users);
    Assert.Empty(users);
  }

  [Fact]
  public async Task GetAllUsers_Returns_List_When_Users_Exist()
  {
    UserController controller = new(GetUserService_ReturnsGoodData(), new MockEmailService_DoesNothing());

    OkObjectResult? result = (await controller.GetAllUsers()).Result as OkObjectResult;
    Assert.NotNull(result);

    IEnumerable<UserViewModel>? users = result.Value as IEnumerable<UserViewModel>;
    Assert.NotNull(users);
    Assert.NotEmpty(users);
  }

  [Fact]
  public async Task GetUserById_Returns_NotFound_When_Empty()
  {
    UserController controller = new(GetUserService_ReturnsNull(), new MockEmailService_DoesNothing());
    NotFoundResult? result = (await controller.GetUserById(ObjectId.Empty.ToString())).Result as NotFoundResult;
    Assert.NotNull(result);
  }

  [Fact]
  public async Task GetUserById_Returns_NotFound_When_Not_Exist()
  {
    UserController controller = new(GetUserService_ReturnsGoodData(), new MockEmailService_DoesNothing());
    NotFoundResult? result = (await controller.GetUserById(ObjectId.Empty.ToString())).Result as NotFoundResult;
    Assert.NotNull(result);
  }

  [Fact]
  public async Task GetUserById_Returns_User_When_Exist()
  {
    IUserService service = GetUserService_ReturnsGoodData();
    ObjectId id = ((List<UserModel>)await service.GetAllUsers())[0].Id;

    UserController controller = new(service, new MockEmailService_DoesNothing());
    OkObjectResult? result = (await controller.GetUserById(id.ToString())).Result as OkObjectResult;
    Assert.NotNull(result);

    UserViewModel? user = result.Value as UserViewModel;
    Assert.NotNull(user);
  }

  private IUserService GetUserService_ReturnsGoodData()
  {
    return new MockUserService_ReturnsGoodData();
  }

  private IUserService GetUserService_ReturnsNull()
  {
    return new MockUserService_ReturnsNull();
  }
}