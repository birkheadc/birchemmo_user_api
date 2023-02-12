using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BircheMmoUserApi.Controllers;

[ApiController]
[Route("api/debug")]
public class DebugController : ControllerBase
{

  private readonly IEmailService emailService;
  private readonly IUserService userService;

  public DebugController(IEmailService emailService, IUserService userService)
  {
    this.emailService = emailService;
    this.userService = userService;
  }

  [HttpGet]
  public IActionResult TestConnection()
  {
    return Ok("If you can see this, you've reached the debug controller.");
  }

  [HttpPost]
  [Route("test-email")]
  public async Task<IActionResult> SendTestEmail()
  {
    try
    {
      bool didSend = await emailService.SendEmailAsync("Master Colby", "birkheadc@gmail.com", "Test Email", "This is just a test! If you're seeing this, it worked!");
      return didSend ? Ok() : StatusCode(9002);
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpPost]
  [Route("test")]
  public async Task<IActionResult> Test()
  {
    try
    {
      NewUserModel newUser = new(
        new Credentials(
          "oldcheddar",
          "password"
        ),
        "birkheadc@gmail.com"
      );
      UserModel? user = await userService.CreateUser(newUser);
      if (user is null) return StatusCode(9003);

      bool didSend = await emailService.SendVerificationEmail(user);
      return didSend ? Ok() : StatusCode(9002);
    }
    catch(Exception e)
    {
      return BadRequest(e.Message);
    }
  }
}