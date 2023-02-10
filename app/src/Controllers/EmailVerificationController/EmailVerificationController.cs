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

  [HttpGet]
  [Route("{verificationCode}")]
  public async Task<IActionResult> VerifyEmailGet([FromRoute] string verificationCode)
  {
    try
    {
      TokenWrapper token = new(verificationCode);
      bool isVerified = await emailVerificationService.ValidateUser(token);
      if (isVerified == true) return Ok("Your code worked!");
      return Ok("Your code didn't work :(");
    }
    catch
    {
      return Ok("Something went wrong!");
    }
  }

  [HttpPost]
  [Route("{verificationCode}")]
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
}