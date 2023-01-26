using BircheMmoUserApi.Filters;
using BircheMmoUserApi.Models;
using Microsoft.AspNetCore.Mvc;

namespace BircheMmoUserApi.Controllers;

[ApiController]
[Route("api/session")]
public class SessionController : ControllerBase
{
  [HttpGet]
  [BasicAuth]
  #pragma warning disable 1998
  public async Task<IActionResult> GenerateSessionToken()
  {
    try
    {
      if (HttpContext.Request.Headers.TryGetValue("Authorization", out var basic) == false)
      {
        return StatusCode(9001);
      }
      Credentials credentials = CredentialsDecoder.DecodeCredentialsFromBasic(basic);
      return Ok("This should have been a token for: " + credentials.Username);
    }
    catch
    {
      return StatusCode(9002);
    }
  }
}