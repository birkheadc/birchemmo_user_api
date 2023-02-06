using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BircheMmoUserApiIntegrationTests;

public class ApplicationTests : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> factory;

  public ApplicationTests(WebApplicationFactory<Program> factory)
  {
    this.factory = factory;
  }

  [Fact]
  public void ApplicationResolves()
  {
    HttpClient client = factory.CreateClient();
    
    Assert.NotNull(client);
  }
}