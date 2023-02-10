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
    Credentials credentials = new(
      "oldcheddar",
      "password"
    );

    WebApplicationFactory<Program> app = new MockWebApplicationFactoryBuilder()
      .WithSeedUser(credentials, Role.UNVALIDATED_USER)
      .Build();
    HttpClient client = app.CreateClient();

    TokenWrapper token = await GetSessionTokenFromApi(client, credentials);
    Assert.NotNull(token);
    
    UserViewModel userViewModel = await GetUserSelfFromApi(client, token);
    Assert.NotNull(userViewModel);
    Assert.Equal(credentials.Username, userViewModel.UserDetails.Username);
  }

  [Fact]
  public async Task GetAllUsers_Returns_401_Unauthorized_If_Not_Admin()
  {
    Credentials credentials = new(
      "oldcheddar",
      "password"
    );

    WebApplicationFactory<Program> app = new MockWebApplicationFactoryBuilder()
      .WithSeedUser(credentials, Role.UNVALIDATED_USER)
      .Build();
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
    Credentials credentials = new(
      "oldcheddar",
      "password"
    );

    WebApplicationFactory<Program> app = new MockWebApplicationFactoryBuilder()
      .WithSeedUser(credentials, Role.UNVALIDATED_ADMIN)
      .Build();
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

  private string ToBasicAuth(string username, string password)
  {
    return System.Convert.ToBase64String(Encoding.GetEncoding("iso-8859-1").GetBytes(username + ":" + password));
  }
}