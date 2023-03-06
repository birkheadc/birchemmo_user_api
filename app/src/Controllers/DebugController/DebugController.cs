using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace BircheMmoUserApi.Controllers;

[ApiController]
[Route("user-api/debug")]
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
}