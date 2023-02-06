using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

namespace BircheMmoUserApiIntegrationTests.Mocks;

public class MockWebApplicationFactory_SeedsGivenUsers<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  
}