using BircheMmoUserApi.Filters;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BircheMmoUserApi.Controllers;

#pragma warning disable 1998
[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{
  private readonly IUserViewService userService;
  private readonly UserConverter converter;

  public UserController(IUserViewService userService)
  {
    this.userService = userService;
    converter = new();
  }

  [HttpGet]
  [Route("all")]
  [SessionTokenAuthorize]
  public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllUsers()
  {
    try
    {
      Console.WriteLine("Get All Users Called!");
      
      IEnumerable<UserViewModel> users = await userService.GetAllUsers();
      return Ok(users);
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpGet]
  [Route("id/{id}")]
  public async Task<ActionResult<UserViewModel>> GetUserById([FromRoute] string id)
  {
    try
    {
      UserViewModel? user = await userService.GetUserById(id);
      if (user is null) return NotFound();
      return Ok(user);
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpGet]
  [SessionTokenAuthorize]
  public async Task<ActionResult<UserViewModel>> GetUserSelf()
  {
    UserViewModel? requestUser = GetRequestUser();
    if (requestUser is null) return Unauthorized();
    return Ok(requestUser);
  }

  [HttpPost]
  [Route("new")]
  public async Task<ActionResult<UserViewModel>> CreateUser([FromBody] NewUserModel user)
  {
    try
    {
      UserViewModel? userViewModel = await userService.CreateUser(user);
      if (userViewModel is null) return StatusCode(9002);
      return Ok(userViewModel);
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpDelete]
  [Route("delete/{id}")]
  public async Task<IActionResult> DeleteUserById([FromRoute] ObjectId id)
  {
    try
    {
      await userService.DeleteUserById(id);
      return Ok();
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpDelete]
  public async Task<IActionResult> DeleteUserSelf()
  {
    // Todo
    return NotFound();
  }

  [HttpPut]
  public async Task<IActionResult> UpdateUserSelf(UserViewModel updatedUser)
  {
    // Todo
    return NotFound();
  }

  private UserViewModel? GetRequestUser()
  {
    try
    {
      UserModel? userModel = HttpContext.Items["requestUser"] as UserModel;
      if (userModel is null) return null;
      return converter.ToUserViewModel(userModel);
    }
    catch
    {
      return null;
    }
  }
}