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

  public EmailVerificationController(IEmailVerificationService emailVerificationService)
  {
    this.emailVerificationService = emailVerificationService;
  }

  [HttpPost]
  [Route("{code}")]
  public async Task<IActionResult> VerifyEmail([FromRoute] string verificationCode)
  {
    try
    {
      TokenWrapper token = new(verificationCode);
      // bool isVerified = await emailVerificationService.ValidateEmailVerificationTokenForUser(token);
      // if (isVerified == true) return Ok();
      return BadRequest();
    }
    catch
    {
      return StatusCode(9001);
    }
  }
}