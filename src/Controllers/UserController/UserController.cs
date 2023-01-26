using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BircheMmoUserApi.Controllers;

[ApiController]
[Route("api/user")]
public class UserController : ControllerBase
{

  private readonly IUserService userService;

  public UserController(IUserService userService)
  {
    this.userService = userService;
  }

  [HttpGet]
  [Route("all")]
  public async Task<IActionResult> GetAllUsers()
  {
    try
    {
      IEnumerable<UserModel> users = await userService.GetAllUsers();
      return Ok(users);
    }
    catch
    {
      return StatusCode(9001);
    }
  }
}