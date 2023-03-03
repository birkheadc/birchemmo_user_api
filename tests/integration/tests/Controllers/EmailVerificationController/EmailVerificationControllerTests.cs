using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Services;
using BircheMmoUserApiIntegrationTests.Mocks.Factories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Xunit;

namespace BircheMmoUserApiIntegrationTests.Controllers;

public class EmailVerificationControllerTests
{
  [Fact]
  public async Task Validate_Returns_401_Unauthorized_With_Bad_Token()
  {
    WebApplicationFactory<Program> app = new MockWebApplicationFactoryBuilder()
      .Build();
    HttpClient client = app.CreateClient();

    TokenWrapper token = new("badtoken");

    HttpRequestMessage request = new(HttpMethod.Post, $"/api/email-verification/verify");
    request.Content = JsonContent.Create(token);
    HttpResponseMessage response = await client.SendAsync(request);
  
    Assert.NotNull(response);
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task Validate_Updates_UserModel_Role_To_VALIDATED_USER()
  {
    Credentials credentials = new(
      "oldcheddar",
      "password"
    );
    WebApplicationFactory<Program> app = new MockWebApplicationFactoryBuilder()
      .WithSeedUser(credentials, Role.UNVALIDATED_USER)
      .Build();
    HttpClient client = app.CreateClient();

    IServiceScope scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
    IUserService userService = scope.ServiceProvider.GetRequiredService<IUserService>();
    UserModel? user = await userService.GetUserByUsername(credentials.Username);
    Assert.NotNull(user);
    Assert.Equal(Role.UNVALIDATED_USER, user.UserDetails.Role);

    IEmailVerificationService emailVerificationService = scope.ServiceProvider.GetRequiredService<IEmailVerificationService>();
    TokenWrapper? token = await emailVerificationService.GenerateForUser(user);
    Assert.NotNull(token);

    HttpRequestMessage request = new(HttpMethod.Post, "/api/email-verification/verify/");
    request.Content = JsonContent.Create(token);
    HttpResponseMessage response = await client.SendAsync(request);

    Assert.NotNull(response);
    response.EnsureSuccessStatusCode();
    user = await userService.GetUserByUsername(credentials.Username);
    Assert.NotNull(user);
    Assert.Equal(Role.VALIDATED_USER, user.UserDetails.Role);
  }
}