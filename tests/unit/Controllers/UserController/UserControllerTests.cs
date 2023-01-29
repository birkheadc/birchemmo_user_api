using System.Collections.Generic;
using BircheMmoUserApi.Controllers;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using BircheMmoUserApiTests.Mocks.Services;
using Microsoft.AspNetCore.Mvc;
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

    IActionResult result = (await controller.GetAllUsers());
    List<UserViewModel> users = result
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