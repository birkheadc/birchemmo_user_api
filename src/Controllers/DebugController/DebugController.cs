using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BircheMmoUserApi.Controllers;

[ApiController]
[Route("/api/debug")]
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
      await emailService.SendTestEmailAsync("Master Colby", "birkheadc@gmail.com");
      return Ok();
    }
    catch
    {
      return StatusCode(9001);
    }
  }
}