using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Repositories;

#pragma warning disable 1998
/// <summary>
/// <c>InMemoryUserRepository</c> should only be used in development and testing.
/// It acts as a quick and dirty database for Users.
/// Make sure to register it as a Singleton, because the "database" will be lost upon reinstantiation.
///</summary>
public class InMemoryUserRepository : IUserRepository
{
  private Dictionary<ObjectId, UserModel> users;

  public InMemoryUserRepository()
  {
    Console.WriteLine("In Memory User Repo Online...");
    users = new();
  }
  public async Task<UserModel?> CreateUser(UserModel user)
  {
    if (IsUsernameAvailable(user.UserDetails.Username) == false) return null;
    users.Add(user.Id, user);
    return user;
  }

  public async Task DeleteUserById(ObjectId id)
  {
    users.Remove(id);
  }

  public async Task EditUser(UserViewModel user)
  {
    if (users.ContainsKey(ObjectId.Parse(user.Id)) == false) return;
    users[ObjectId.Parse(user.Id)].UserDetails.Username = user.UserDetails.Username;
    users[ObjectId.Parse(user.Id)].UserDetails.Role = user.UserDetails.Role;
    users[ObjectId.Parse(user.Id)].IsEmailVerified = user.IsEmailVerified;
  }

  public async Task<IEnumerable<UserModel>> FindAllUsers()
  {
    foreach (UserModel user in users.Values)
    {
      Console.WriteLine("--------------------");
      Console.WriteLine("ID: " + user.Id);
      Console.WriteLine("Username: " + user.UserDetails.Username);
      Console.WriteLine("HashedPassword: " + user.HashedPassword);
      Console.WriteLine("Role: " + user.UserDetails.Role);
      Console.WriteLine("IsEmailVerified: " + user.IsEmailVerified);
      Console.WriteLine("--------------------");
      Console.WriteLine("");
    }
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
      return users.Where(user => user.Value.UserDetails.Username == username).First().Value;
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
      if (pair.Value.UserDetails.Username == username) return false;
    }
    return true;
  }
}