using System.Net.Http;
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace BircheMmoUserApiIntegrationTests;

public class ApplicationResolves : IClassFixture<WebApplicationFactory<Program>>
{
  private readonly WebApplicationFactory<Program> factory;

  public ApplicationResolves(WebApplicationFactory<Program> factory)
  {
    this.factory = factory;
  }

  [Fact]
  public void ApplicationResolvesPlease()
  {
    HttpClient client = factory.CreateClient();
    
    Assert.NotNull(client);
  }
}