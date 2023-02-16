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
  private readonly IUserService userService;
  private readonly UserConverter converter;
  private readonly IEmailService emailService;

  public UserController(IUserService userService, IEmailService emailService)
  {
    this.userService = userService;
    converter = new();
    this.emailService = emailService;
  }

  [HttpGet]
  [Route("all")]
  [SessionTokenAuthorizeAttribute(Role.ADMIN)]
  public async Task<ActionResult<IEnumerable<UserViewModel>>> GetAllUsers()
  {
    try
    {
      Console.WriteLine("Get All Users Called!");
      
      IEnumerable<UserModel> users = await userService.GetAllUsers();

      return Ok(converter.ToUserViewModels(users));
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpGet]
  [Route("id/{id}")]
  [SessionTokenAuthorize(Role.ADMIN)]
  public async Task<ActionResult<UserViewModel>> GetUserById([FromRoute] string id)
  {
    try
    {
      UserModel? user = await userService.GetUserById(ObjectId.Parse(id));
      if (user is null) return NotFound();
      return Ok(converter.ToUserViewModel(user));
    }
    catch
    {
      return StatusCode(9001);
    }
  } 

  [HttpGet]
  [SessionTokenAuthorize(Role.UNVALIDATED_USER)]
  public async Task<ActionResult<UserViewModel>> GetUserSelf()
  {
    UserModel? requestUser = GetRequestUser();
    if (requestUser is null) return Unauthorized();
    return Ok(converter.ToUserViewModel(requestUser));
  }

  [HttpPost]
  [Route("new")]
  public async Task<ActionResult<UserViewModel>> CreateUser([FromBody] NewUserModel user)
  {
    try
    {
      UserModel? userModel = await userService.CreateUser(user);
      if (userModel is null) return StatusCode(9002);
      await emailService.SendVerificationEmail(userModel);
      return Ok(converter.ToUserViewModel(userModel));
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpPost]
  [Route("new-admin")]
  [SessionTokenAuthorize(Role.SUPER_ADMIN)]
  public async Task<ActionResult<UserViewModel>> CreateAdmin([FromBody] NewUserModel user)
  {
    try
    {
      // Todo: does this even create an admin???
      UserModel? userModel = await userService.CreateUser(user);
      if (userModel is null) return StatusCode(9002);
      return Ok(converter.ToUserViewModel(userModel));
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpDelete]
  [Route("delete/{id}")]
  [SessionTokenAuthorize(Role.ADMIN)]
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
  [SessionTokenAuthorize(Role.UNVALIDATED_USER)]
  public async Task<IActionResult> DeleteUserSelf()
  {
    // Todo
    return NotFound();
  }

  [HttpPut]
  [SessionTokenAuthorize(Role.UNVALIDATED_USER)]
  public async Task<IActionResult> UpdateUserSelf(UserViewModel updatedUser)
  {
    // Todo
    return NotFound();
  }

  private UserModel? GetRequestUser()
  {
    try
    {
      UserModel? userModel = HttpContext.Items["requestUser"] as UserModel;
      return userModel;
    }
    catch
    {
      return null;
    }
  }
}