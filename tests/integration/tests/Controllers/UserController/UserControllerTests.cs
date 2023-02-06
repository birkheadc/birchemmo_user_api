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
using System;

namespace BircheMmoUserApiIntegrationTests.Controllers;

public class UserControllerTests
{
  [Fact]
  public async Task GetUserSelf_Returns_401_Unauthorized_If_Not_Authorized()
  {
    WebApplicationFactory<Program> app = new();
    HttpClient client = app.CreateClient();

    HttpResponseMessage response = await client.GetAsync("/api/user");
    Assert.NotNull(response);
    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task Existing_User_Is_Able_To_Generate_Session_Token_Then_View_Their_Own_UserViewModel()
  {
    List<UserModel> seedUsers = new();
    Credentials credentials = new(
      "oldcheddar",
      "password"
    );
    UserModel user = GenerateUser(credentials, Role.UNVALIDATED_USER);
    seedUsers.Add(user);

    WebApplicationFactory<Program> app = await CreateMockAppWithSeedUsers(seedUsers);
    HttpClient client = app.CreateClient();

    TokenWrapper token = await GetSessionTokenFromApi(client, credentials);
    Assert.NotNull(token);
    
    UserViewModel userViewModel = await GetUserSelfFromApi(client, token);
    Assert.NotNull(userViewModel);
    Assert.Equal(user.Id.ToString(), userViewModel.Id);
  }

  [Fact]
  public async Task GetAllUsers_Returns_401_Unauthorized_If_Not_Admin()
  {
    List<UserModel> seedUsers = new();
    Credentials credentials = new(
      "oldcheddar",
      "password"
    );
    UserModel user = GenerateUser(credentials, Role.UNVALIDATED_USER);
    seedUsers.Add(user);

    WebApplicationFactory<Program> app = await CreateMockAppWithSeedUsers(seedUsers);
    HttpClient client = app.CreateClient();

    TokenWrapper token = await GetSessionTokenFromApi(client, credentials);
    Assert.NotNull(token);

    HttpRequestMessage request = new(HttpMethod.Get, "/api/user/all");
    request.Headers.Authorization = new(
      "Bearer",
      token.Token
    );

    HttpResponseMessage response = await client.SendAsync(request);

    Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
  }

  [Fact]
  public async Task GetAllUsers_Returns_All_Users_If_Admin()
  {
    List<UserModel> seedUsers = new();
    Credentials credentials = new(
      "oldcheddar",
      "password"
    );
    UserModel user = GenerateUser(credentials, Role.ADMIN);
    seedUsers.Add(user);

    WebApplicationFactory<Program> app = await CreateMockAppWithSeedUsers(seedUsers);
    HttpClient client = app.CreateClient();

    TokenWrapper token = await GetSessionTokenFromApi(client, credentials);
    Assert.NotNull(token);

    HttpRequestMessage request = new(HttpMethod.Get, "/api/user/all");
    request.Headers.Authorization = new(
      "Bearer",
      token.Token
    );

    HttpResponseMessage response = await client.SendAsync(request);

    Assert.NotNull(response);
    response.EnsureSuccessStatusCode();

    string content = await response.Content.ReadAsStringAsync();
    List<UserViewModel> userViewModels = JsonConvert.DeserializeObject<List<UserViewModel>>(content);

    Assert.Equal(HttpStatusCode.OK, response.StatusCode);

    Assert.True(userViewModels.Count == 1);
    Assert.Equal("oldcheddar", userViewModels[0].UserDetails.Username);
  }

  private async Task<TokenWrapper> GetSessionTokenFromApi(HttpClient client, Credentials credentials)
  {
    HttpRequestMessage request = new(HttpMethod.Get, "/api/session");
    request.Headers.Authorization = new(
      "Basic",
      credentials.ToBasicAuth()
    );
    HttpResponseMessage response = await client.SendAsync(request);

    Assert.NotNull(response);
    response.EnsureSuccessStatusCode();
    
    string content = await response.Content.ReadAsStringAsync();
    TokenWrapper token = JsonConvert.DeserializeObject<TokenWrapper>(content);

    return token;
  }

  private async Task<UserViewModel> GetUserSelfFromApi(HttpClient client, TokenWrapper token)
  {
    HttpRequestMessage request = new(HttpMethod.Get, "/api/user");
    request.Headers.Authorization = new(
      "Bearer",
      token.Token
    );

    HttpResponseMessage response = await client.SendAsync(request);

    Assert.NotNull(response);
    response.EnsureSuccessStatusCode();

    string content = await response.Content.ReadAsStringAsync();
    UserViewModel userViewModel = JsonConvert.DeserializeObject<UserViewModel>(content);

    return userViewModel;
  }

  private async Task<List<UserViewModel>?> GetAllUsersFromApi(HttpClient client, TokenWrapper token)
  {
    HttpRequestMessage request = new(HttpMethod.Get, "/api/user/all");
    request.Headers.Authorization = new(
      "Bearer",
      token.Token
    );

    HttpResponseMessage response = await client.SendAsync(request);

    Assert.NotNull(response);
    try
    {
      response.EnsureSuccessStatusCode();
    }
    catch
    {
      return null;
    }
    string content = await response.Content.ReadAsStringAsync();
    List<UserViewModel> userViewModels = JsonConvert.DeserializeObject<List<UserViewModel>>(content);

    return userViewModels;
  }

  private async Task<WebApplicationFactory<Program>> CreateMockAppWithSeedUsers(List<UserModel> seedUsers)
  {
    WebApplicationFactory<Program> app = new();

    IUserRepository repository = app.Services.GetRequiredService<IUserRepository>();
    foreach (UserModel user in seedUsers)
    {
      await repository.CreateUser(user);
    }
    return app;
  }

  private UserModel GenerateUser(Credentials credentials, Role role)
  {
    return new UserModel(
      ObjectId.GenerateNewId(),
      credentials.Username,
      BCrypt.Net.BCrypt.HashPassword(credentials.Password),
      "oldcheddar@site.net",
      role,
      false
    );
  }

  private string ToBasicAuth(string username, string password)
  {
    return System.Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes(username + ":" + password));
  }
}