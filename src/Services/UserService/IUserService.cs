using BircheMmoUserApi.Models;

namespace BircheMmoUserApi.Services;

public interface IUserService
{
  public Task<UserModel> GetUserByUsername(string Username);
  public Task<IEnumerable<UserModel>> GetAllUsers();
}