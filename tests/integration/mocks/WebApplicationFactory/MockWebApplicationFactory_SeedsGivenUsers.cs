using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;

namespace BircheMmoUserApiIntegrationTests.Mocks;

public class MockWebApplicationFactory_SeedsGivenUsers<TProgram> : WebApplicationFactory<TProgram> where TProgram : class
{
  // private readonly List<UserModel> seedUsers;

  public MockWebApplicationFactory_SeedsGivenUsers()
  {
    // this.seedUsers = seedUsers;
    Console.WriteLine("Constructed!");
  }
  // protected override void ConfigureWebHost(IWebHostBuilder builder)
  // {
  //   builder.ConfigureServices(
  //     services =>
  //     {
  //       IUserRepository? userRepository = services.SingleOrDefault(
  //         service => service.ServiceType == typeof(IUserRepository)
  //       ) as IUserRepository;
  //       if (userRepository is not null)
  //       {
  //         foreach (UserModel user in seedUsers)
  //         {
  //           userRepository.CreateUser(user);
  //         }
  //       }
  //     }
  //   );
  // }
}