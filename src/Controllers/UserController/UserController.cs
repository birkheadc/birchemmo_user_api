using BircheMmoUserApi.Exceptions;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BircheMmoUserApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{

  private readonly IUserViewService userService;

  public UserController(IUserViewService userService)
  {
    this.userService = userService;
  }

  [HttpGet]
  [Route("all")]
  public async Task<IActionResult> GetAllUsers()
  {
    try
    {
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
  public async Task<IActionResult> GetUserById([FromRoute] ObjectId id)
  {
    try
    {
      UserViewModel user = await userService.GetUserById(id);
      return Ok(user);
    }
    catch (UserNotFoundException)
    {
      return NotFound();
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpPost]
  [Route("new")]
  public async Task<IActionResult> CreateUser([FromBody] NewUserModel user)
  {
    try
    {
      await userService.CreateUser(user);
      return Ok();
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpDelete]
  [Route("delete/{id}")]
  public async Task<IActionResult> DeleteUser([FromRoute] ObjectId id)
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
}