using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BircheMmoUserApi.Controllers;

[ApiController]
[Route("api/debug")]
public class DebugController : ControllerBase
{

  private readonly IEmailService emailService;

  public DebugController(IEmailService emailService)
  {
    this.emailService = emailService;
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
}