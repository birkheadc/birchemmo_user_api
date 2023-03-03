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
    users = new();
  }
  public async Task<UserModel?> CreateUser(UserModel user)
  {
    if (IsUserUniqueEnough(user) == false) return null;

    users.Add(user.Id, user);
    return user;
  }

  public async Task DeleteUserById(ObjectId id)
  {
    users.Remove(id);
  }

  public async Task UpdateUserDetails(ObjectId id, UserDetails userDetails)
  {
    if (users.ContainsKey(id) == false) return;
    users[id].UserDetails = userDetails;
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

  private bool IsUserUniqueEnough(UserModel user)
  {
    foreach (KeyValuePair<ObjectId, UserModel> pair in users)
    {
      if (pair.Value.UserDetails.Username == user.UserDetails.Username) return false;
      if (pair.Value.UserDetails.EmailAddress == user.UserDetails.EmailAddress) return false;
    }
    return true;
  }

  public async Task UpdatePassword(ObjectId id, string newPassword)
  {
    if (users.ContainsKey(id) == false) return;
    users[id].HashedPassword = BCrypt.Net.BCrypt.HashPassword(newPassword);
  }

  public async Task<UserModel?> FindUserByEmailAddress(string emailAddress)
  {
    try
    {
      return users.Where(user => user.Value.UserDetails.EmailAddress == emailAddress).First().Value;
    }
    catch
    {
      return null;
    }
  }
}