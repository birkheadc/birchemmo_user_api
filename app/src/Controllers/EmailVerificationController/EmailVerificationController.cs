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

  public EmailVerificationController(IEmailVerificationService emailVerificationService, IEmailService emailService)
  {
    this.emailVerificationService = emailVerificationService;
    this.emailService = emailService;
  }

  [HttpPost]
  [Route("verify/{verificationCode}")]
  public async Task<IActionResult> VerifyEmail([FromRoute] string verificationCode)
  {
    try
    {
      TokenWrapper token = new(verificationCode);
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
  [Route("send")]
  [SessionTokenAuthorize(Role.UNVALIDATED_USER)]
  public async Task<IActionResult> SendEmail()
  {
    UserModel? user = HttpContext.Items["requestUser"] as UserModel;
    if (user is null) return Unauthorized();
    bool didSend = await emailService.SendVerificationEmail(user);
    if (didSend == true) return Ok();
    return BadRequest();
  }
}