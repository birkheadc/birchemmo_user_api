using System.Collections.Generic;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Bson;

namespace BircheMmoUserApiIntegrationTests.Mocks;

public class MockWebApplicationFactoryBuilder
{
  private List<UserModel> seedUsers = new();
  public MockWebApplicationFactoryBuilder WithSeedUser(Credentials credentials, Role role, string email = "example@place.com")
  {
    seedUsers.Add(
      new UserModel(
        ObjectId.GenerateNewId(),
        credentials.Username,
        BCrypt.Net.BCrypt.HashPassword(credentials.Password),
        email,
        role
      )
    );

    return this;
  }

  public WebApplicationFactory<Program> Build()
  {
    WebApplicationFactory<Program> app = new();

    if (seedUsers.Count > 0)
    {
      IUserRepository userRepository = app.Services.GetRequiredService<IUserRepository>();
      foreach (UserModel user in seedUsers)
      {
        userRepository.CreateUser(user);
      }
    }

    return app;
  }
}