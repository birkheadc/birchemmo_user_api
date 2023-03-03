using BircheMmoUserApi.Filters;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BircheMmoUserApi.Controllers;

#pragma warning disable 1998
[ApiController]
[Route("api/email-verification")]
public class EmailVerificationController : ControllerBase
{
  private readonly IEmailVerificationService emailVerificationService;
  private readonly IEmailService emailService;
  private readonly IUserService userService;

  public EmailVerificationController(IEmailVerificationService emailVerificationService, IEmailService emailService, IUserService userService)
  {
    this.emailVerificationService = emailVerificationService;
    this.emailService = emailService;
    this.userService = userService;
  }

  [HttpPost]
  [Route("verify")]
  public async Task<IActionResult> VerifyEmail([FromBody] TokenWrapper token)
  {
    try
    {
      bool isVerified = await emailVerificationService.ValidateUser(token);
      if (isVerified == true) return Ok();
      return Unauthorized();
    }
    catch
    {
      return StatusCode(9001);
    }
  }

  [HttpPost]
  [Route("send/{emailAddress}")]
  public async Task<IActionResult> SendEmail([FromRoute] string emailAddress)
  {
    Console.WriteLine("Attempt to resend verification to: " + emailAddress);
    UserModel? user = await userService.GetUserByEmailAddress(emailAddress);
    if (user is null) return Unauthorized();
    if (user.UserDetails.Role > Role.UNVALIDATED_USER) return BadRequest();
    bool didSend = await emailService.SendVerificationEmail(user);
    if (didSend == true) return Ok();
    return BadRequest();
  }
}