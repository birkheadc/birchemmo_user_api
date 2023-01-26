using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;

namespace BircheMmoUserApi.Services;

public class UserService : IUserService
{
  private readonly IUserRepository userRepository;

  public UserService(IUserRepository userRepository)
  {
    this.userRepository = userRepository;
  }

  public async Task<IEnumerable<UserModel>> GetAllUsers()
  {
    throw new NotImplementedException();
  }

  public async Task<UserModel> GetUserByUsername(string Username)
  {
    throw new NotImplementedException();
  }
}