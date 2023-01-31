using BircheMmoUserApi.Config;
using BircheMmoUserApi.Models;
using MongoDB.Bson;

namespace BircheMmoUserApi.Repositories;

public class MongoDbUserRepository : IUserRepository
{
  private readonly DbConfig dbConfig;

  public MongoDbUserRepository(DbConfig dbConfig)
  {
    this.dbConfig = dbConfig;
  }

  public Task<UserModel?> CreateUser(UserModel user)
  {
    throw new NotImplementedException();
  }

  public Task DeleteUserById(ObjectId id)
  {
    throw new NotImplementedException();
  }

  public Task EditUser(UserViewModel user)
  {
    throw new NotImplementedException();
  }

  public Task<IEnumerable<UserModel>> FindAllUsers()
  {
    throw new NotImplementedException();
  }

  public Task<UserModel?> FindUserById(ObjectId id)
  {
    throw new NotImplementedException();
  }

  public Task<UserModel?> FindUserByUsername(string username)
  {
    throw new NotImplementedException();
  }
}