using BircheMmoUserApi.Controllers;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using MongoDB.Bson;

namespace BircheMmoUserApi.Filters;

public class SessionTokenAuthorizeAttribute : Attribute, IAsyncActionFilter
{
  public SessionTokenAuthorizeAttribute()
  {
    // Todo: Add action parameters to constructor so we can decide if the request user is allowed to do this thing.
  }
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    // Refuse access if no password is included in request
    if (context.HttpContext.Request.Headers.TryGetValue("Authorization", out var bearer) == false)
    {
      context.Result = new UnauthorizedResult();
      return;
    }

    // Refuse access if Authorization header is formatted wrong
    if (bearer.ToString().StartsWith("Bearer ") == false)
    {
      context.Result = new BadRequestResult();
      return;
    }

    // Validate token, deny if not valid
    TokenWrapper token = GetTokenFromBearer(bearer);
    ISessionService sessionService = context.HttpContext.RequestServices.GetRequiredService<ISessionService>();
    UserModel? requestUser = await sessionService.ValidateSessionToken(token);
    if (requestUser is null)
    {
      context.Result = new UnauthorizedResult();
      return;
    }
  
    // Todo: Check if requestUser is authorized to do the thing
    context.HttpContext.Items["requestUser"] = requestUser;
    await next();
  }
  
  private TokenWrapper GetTokenFromBearer(string bearer)
  {
    return new TokenWrapper(
      bearer.Substring("Bearer ".Length)
    );
  }
}