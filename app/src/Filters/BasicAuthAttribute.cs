using System.Text;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BircheMmoUserApi.Filters;

public class BasicAuthAttribute : Attribute, IAsyncActionFilter
{
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    // Refuse access if no password is included in request
    if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var basic) == false)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    // Refuse access if Authorization header is formatted wrong
    if (basic.ToString().StartsWith("Basic ") == false)
    {
      context.Result = new BadRequestResult();
      return;
    }

    // Check credentials, refuse access if not valid
    Credentials? credentials = GetCredentialsFromBasic(basic);
    if (credentials is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }
    IUserService userService = context.HttpContext.RequestServices.GetRequiredService<IUserService>();
    UserModel? user = await userService.GetUserByUsername(credentials.Username);
    
    if (AreCredentialsValid(user, credentials) == false)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    // Allow access
    context.ActionArguments["credentials"] = credentials;
    await next();
  }

  private Credentials? GetCredentialsFromBasic(string basic)
  {
    CredentialsDecoder decoder = new();
    return decoder.DecodeCredentialsFromBasic(basic);
  }

  private bool AreCredentialsValid(UserModel? user, Credentials credentials)
  {
    if (user is null) return false;
    return BCrypt.Net.BCrypt.Verify(credentials.Password, user.HashedPassword);
  }
}