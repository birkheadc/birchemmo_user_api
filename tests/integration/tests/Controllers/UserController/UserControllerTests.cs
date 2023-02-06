using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;
using BircheMmoUserApiIntegrationTests.Mocks;
using System.Collections.Generic;
using BircheMmoUserApi.Models;
using MongoDB.Bson;
using System.Net.Http.Headers;
using System.Text;
using Microsoft.Extensions.DependencyInjection;
using BircheMmoUserApi.Repositories;
using Newtonsoft.Json;

namespace BircheMmoUserApiIntegrationTests.Controllers;

public class UserControllerTests
{
  [Fact]
  public async Task GetUserSelf_Returns_401_Unauthorized_If_Not_Authorized()
  {
    WebApplicationFactory<Program> app = new();

    HttpClient client = app.CreateClient();
    Assert.NotNull(client);

    HttpResponseMessage response = await client.GetAsync("/api/user");
    Assert.NotNull(response);
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task Existing_User_Is_Able_To_Generate_Session_Token_Then_View_Their_Own_UserViewModel()
  {
    List<UserModel> seedUsers = new();
    string user_1_username = "oldcheddar";
    string user_1_password = "password";
    UserModel user_1 = new(
      ObjectId.GenerateNewId(),
      user_1_username,
      BCrypt.Net.BCrypt.HashPassword(user_1_password),
      "oldcheddar@site.net",
      Role.UNVALIDATED_USER,
      false
    );
    seedUsers.Add(user_1);

    WebApplicationFactory<Program> app = new();

    IUserRepository repository = app.Services.GetRequiredService<IUserRepository>();
    foreach (UserModel user in seedUsers)
    {
      await repository.CreateUser(user);
    }

    HttpClient client = app.CreateClient();
    Assert.NotNull(client);

    HttpRequestMessage message = new(HttpMethod.Get, "/api/session");
    message.Headers.Authorization = new(
      "Basic",
      System.Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes(user_1_username + ":" + user_1_password))
    );
    HttpResponseMessage response = await client.SendAsync(message);

    Assert.NotNull(response);
    response.EnsureSuccessStatusCode();
    
    string content = await response.Content.ReadAsStringAsync();
    TokenWrapper token = JsonConvert.DeserializeObject<TokenWrapper>(content);

    Assert.NotNull(token);
    
    message = new(HttpMethod.Get, "/api/user");
    message.Headers.Authorization = new(
      "Bearer",
      token.Token
    );

    response = await client.SendAsync(message);

    Assert.NotNull(response);
    response.EnsureSuccessStatusCode();

    content = await response.Content.ReadAsStringAsync();
    UserViewModel userViewModel = JsonConvert.DeserializeObject<UserViewModel>(content);

    Assert.NotNull(userViewModel);
    Assert.Equal(user_1.Id.ToString(), userViewModel.Id);
  }
}