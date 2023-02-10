using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Abstractions;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Routing;
using BircheMmoUserApi.Filters;
using Moq;
using Xunit;
using BircheMmoUserApi.ErrorMessages;
using BircheMmoUserApi.Services;
using BircheMmoUserApiUnitTests.Mocks.Services;
using BircheMmoUserApiUnitTests.Mocks.Config;
using BircheMmoUserApi.Models;
using BircheMmoUserApiUnitTests.Mocks.Controllers;

namespace BircheMmoUserApiUnitTests.Filters;

#pragma warning disable 1998
public class SessionTokenAuthAttributeTests
{
  [Fact]
  public async Task OnActionExecutionAsync_ActionContext_Result_Is_UnauthorizedResult_When_No_Authorization_Header()
  {
    SessionTokenAuthorizeAttribute sessionTokenAuthorizeAttribute = new(Role.VISITOR);

    HttpContext httpContext = GetMockHttpContext_With_No_Headers();
    ActionContext actionContext = BuildActionContextWithHttpContext(httpContext);
    ActionExecutingContext actionExecutingContext = BuildActionExecutingContextWithActionContext(actionContext);
    ActionExecutionDelegate actionExecutionDelegate = BuildActionExecutionDelegateWithActionContext(actionContext);

    await sessionTokenAuthorizeAttribute.OnActionExecutionAsync(
      actionExecutingContext,
      actionExecutionDelegate
    );

    UnauthorizedObjectResult? result = actionExecutingContext.Result as UnauthorizedObjectResult;
    Assert.NotNull(result);
    Assert.Equal(UnauthorizedErrorMessage.MISSING_AUTHORIZATION_HEADER, result.Value);
  }

  [Fact]
  public async Task OnActionExecutionAsync_ActionContext_Result_Is_UnauthorizedResult_When_Authorization_Header_Is_Wrong_Format()
  {
    SessionTokenAuthorizeAttribute sessionTokenAuthorizeAttribute = new(Role.VISITOR);

    HttpContext httpContext = GetMockHttpContext_With_Bad_Authorization_Header();
    ActionContext actionContext = BuildActionContextWithHttpContext(httpContext);
    ActionExecutingContext actionExecutingContext = BuildActionExecutingContextWithActionContext(actionContext);
    ActionExecutionDelegate actionExecutionDelegate = BuildActionExecutionDelegateWithActionContext(actionContext);

    await sessionTokenAuthorizeAttribute.OnActionExecutionAsync(
      actionExecutingContext,
      actionExecutionDelegate
    );

    UnauthorizedObjectResult? result = actionExecutingContext.Result as UnauthorizedObjectResult;
    Assert.NotNull(result);
    Assert.Equal(UnauthorizedErrorMessage.AUTHORIZATION_HEADER_BAD_FORMAT, result.Value);
  }

  [Fact]
  public async Task OnActionExecutionAsync_ActionContext_Result_Is_UnauthorizedResult_When_Authorization_Header_Contains_Invalid_Token()
  {
    SessionTokenAuthorizeAttribute sessionTokenAuthorizeAttribute = new(Role.VISITOR);

    HttpContext httpContext = GetMockHttpContext_With_Bad_Session_Token_With_SessionService(GetMockSessionService());
    ActionContext actionContext = BuildActionContextWithHttpContext(httpContext);
    ActionExecutingContext actionExecutingContext = BuildActionExecutingContextWithActionContext(actionContext);
    ActionExecutionDelegate actionExecutionDelegate = BuildActionExecutionDelegateWithActionContext(actionContext);

    await sessionTokenAuthorizeAttribute.OnActionExecutionAsync(
      actionExecutingContext,
      actionExecutionDelegate
    );

    UnauthorizedObjectResult? result = actionExecutingContext.Result as UnauthorizedObjectResult;
    Assert.NotNull(result);
    Assert.Equal(UnauthorizedErrorMessage.TOKEN_INVALID, result.Value);
  }

  [Fact]
  public async Task OnActionExecutionAsync_Adds_RequestUser_To_Context_When_Token_Is_Valid()
  {
    ISessionService mockSessionService = GetMockSessionService();
    SessionTokenAuthorizeAttribute sessionTokenAuthorizeAttribute = new(Role.VISITOR);

    Credentials credentials = new(
      "admin",
      "password"
    );

    TokenWrapper? token = await mockSessionService.GenerateSessionToken(credentials);

    Assert.NotNull(token);

    HttpContext httpContext = GetMockHttpContext_With_Good_Session_Token_And_SessionService(token.Token, mockSessionService);
    ActionContext actionContext = BuildActionContextWithHttpContext(httpContext);
    ActionExecutingContext actionExecutingContext = BuildActionExecutingContextWithActionContext(actionContext);
    ActionExecutionDelegate actionExecutionDelegate = BuildActionExecutionDelegateWithActionContext(actionContext);

    await sessionTokenAuthorizeAttribute.OnActionExecutionAsync(
      actionExecutingContext,
      actionExecutionDelegate
    );

    UserModel? requestUser = actionExecutingContext.HttpContext.Items["requestUser"] as UserModel;
    Assert.NotNull(requestUser);

    Assert.Equal(credentials.Username, requestUser.UserDetails.Username);
  }

  [Fact]
  public async Task OnActionExecutionAsync_Result_Is_Unauthorized_When_Role_Is_Insufficient()
  {
    ISessionService mockSessionService = GetMockSessionService();
    SessionTokenAuthorizeAttribute sessionTokenAuthorizeAttribute = new(Role.UNVALIDATED_ADMIN);

    Credentials credentials = new(
      "validated_user",
      "password"
    );

    TokenWrapper? token = await mockSessionService.GenerateSessionToken(credentials);

    Assert.NotNull(token);

    HttpContext httpContext = GetMockHttpContext_With_Good_Session_Token_And_SessionService(token.Token, mockSessionService);
    ActionContext actionContext = BuildActionContextWithHttpContext(httpContext);
    ActionExecutingContext actionExecutingContext = BuildActionExecutingContextWithActionContext(actionContext);
    ActionExecutionDelegate actionExecutionDelegate = BuildActionExecutionDelegateWithActionContext(actionContext);

    await sessionTokenAuthorizeAttribute.OnActionExecutionAsync(
      actionExecutingContext,
      actionExecutionDelegate
    );

    UnauthorizedObjectResult? result = actionExecutingContext.Result as UnauthorizedObjectResult;
    Assert.NotNull(result);
    Assert.Equal(UnauthorizedErrorMessage.PERMISSION_DENIED, result.Value);
  }

  private HttpContext GetMockHttpContext_With_No_Headers()
  {
    Mock<HttpContext> mock = new();
    mock.Setup(context => context.Request.Headers).Returns(new HeaderDictionary());
    return mock.Object;
  }

  private HttpContext GetMockHttpContext_With_Bad_Authorization_Header()
  {
    HeaderDictionary headers = new();
    headers.Add("Authorization", "BadFormat sldakfjalsdkjf");
    Mock<HttpContext> mock = new();
    mock.Setup(context => context.Request.Headers).Returns(headers);
    return mock.Object;
  }

  private HttpContext GetMockHttpContext_With_Bad_Session_Token_With_SessionService(ISessionService sessionService)
  {
    HeaderDictionary headers = new();
    headers.Add("Authorization", "Bearer wow_this_token_is_trash");
    Mock<HttpContext> mock = new();
    mock.Setup(context => context.Request.Headers).Returns(headers);
    mock.Setup(context => context.RequestServices.GetService(typeof(ISessionService))).Returns(sessionService);
    HttpContext context = mock.Object;
    return mock.Object;
  }

  private HttpContext GetMockHttpContext_With_Good_Session_Token_And_SessionService(string token, ISessionService sessionService)
  {
    HeaderDictionary headers = new();
    headers.Add("Authorization", "Bearer " + token);
    Mock<HttpContext> mock = new();
    mock.Setup(context => context.Request.Headers).Returns(headers);
    mock.Setup(context => context.RequestServices.GetService(typeof(ISessionService))).Returns(sessionService);
    mock.Setup(context => context.Items).Returns(new Dictionary<object, object?>());
    return mock.Object;
  }

  private ActionContext BuildActionContextWithHttpContext(HttpContext httpContext)
  {
    Mock<ActionDescriptor> mock = new();
    return new ActionContext(
      httpContext,
      Mock.Of<RouteData>(),
      Mock.Of<ActionDescriptor>(),
      Mock.Of<ModelStateDictionary>()
    );
  }

  private ActionExecutingContext BuildActionExecutingContextWithActionContext(ActionContext actionContext)
  {
    return new ActionExecutingContext(
      actionContext,
      new List<IFilterMetadata>(),
      new Dictionary<string, object?>(),
      new MockController_ReturnsOk()
    );
  }

  private ActionExecutionDelegate BuildActionExecutionDelegateWithActionContext(ActionContext actionContext)
  {
    return () =>
      Task.Run(() => BuildActionExecutedContextWithActionContext(actionContext));
  }

  private ActionExecutedContext BuildActionExecutedContextWithActionContext(ActionContext actionContext)
  {
    return new ActionExecutedContext(
      actionContext,
      new List<IFilterMetadata>(),
      new MockController_ReturnsOk()
    );
  }

  private ISessionService GetMockSessionService()
  {
    return new SessionService(
      new MockUserService_ReturnsGoodData(),
      new JwtService(new MockJwtConfig_GoodData())
    );
  }
}