using BircheMmoUserApi.Filters;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace BircheMmoUserApi.Controllers;

[ApiController]
[Route("api/session")]
public class SessionController : ControllerBase
{
  private readonly ISessionService sessionService;

  public SessionController(ISessionService sessionService)
  {
    this.sessionService = sessionService;
  }

  [HttpGet]
  [ExtractBasicAuth]
  #pragma warning disable 1998
  public async Task<ActionResult<SessionToken>> GenerateSessionToken([FromQuery] Credentials? credentials)
  {
    try
    {
      if (credentials is null) return StatusCode(9003);
      SessionToken? token = await sessionService.GenerateSessionToken(credentials);
      return token is null ? Unauthorized() : Ok(token);
    }
    catch
    {
      return StatusCode(9002);
    }
  }
}