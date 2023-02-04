using Microsoft.AspNetCore.Mvc;

namespace BircheMmoUserApiUnitTests.Mocks.Controllers;

[ApiController]
public class MockController_ReturnsOk : ControllerBase
{
  [HttpGet]
  public ActionResult Get()
  {
    return Ok();
  }
}