using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Repositories;

#pragma warning disable 1998
public class InMemoryUserRepository : IUserRepository
{
  private Dictionary<ObjectId, UserModel> users;

  public InMemoryUserRepository()
  {
    users = new();
  }
  public async Task<UserModel?> CreateUser(NewUserModel newUser)
  {
    if (IsUsernameAvailable(newUser.Username) == false) return null;
    UserModel user = new(
      ObjectId.GenerateNewId(),
      newUser.Username,
      newUser.Password,
      newUser.Role,
      false
    );
    users.Add(user.Id, user);
    return user;
  }

  public async Task DeleteUserById(ObjectId id)
  {
    users.Remove(id);
  }

  public async Task EditUser(UserViewModel user)
  {
    if (users.ContainsKey(user.Id) == false) return;
    users[user.Id].Username = user.Username;
    users[user.Id].Role = user.Role;
    users[user.Id].IsEmailVerified = user.IsEmailVerified;
  }

  public async Task<IEnumerable<UserModel>> FindAllUsers()
  {
    return users.Values;
  }

  public async Task<UserModel?> FindUserById(ObjectId id)
  {
    try
    {
      return users[id];
    }
    catch
    {
      return null;
    }
  }

  public async Task<UserModel?> FindUserByUsername(string username)
  {
    try
    {
      return users.Where(user => user.Value.Username == username).First().Value;
    }
    catch
    {
      return null;
    }
  }

  private bool IsUsernameAvailable(string username)
  {
    foreach (KeyValuePair<ObjectId, UserModel> pair in users)
    {
      if (pair.Value.Username == username) return false;
    }
    return true;
  }
}