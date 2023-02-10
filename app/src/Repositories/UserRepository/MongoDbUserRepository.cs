using BircheMmoUserApi.Config;
using BircheMmoUserApi.Models;
using MongoDB.Bson;
using MongoDB.Driver;

namespace BircheMmoUserApi.Repositories;

public class MongoDbUserRepository : IUserRepository
{
  private readonly IMongoCollection<UserModel> userCollection;

  public MongoDbUserRepository(DbConfig dbConfig)
  {
    MongoClient mongoClient = new(dbConfig.ConnectionString);
    IMongoDatabase database = mongoClient.GetDatabase(dbConfig.DatabaseName);
    userCollection = database.GetCollection<UserModel>(dbConfig.UserCollectionName);
  }

  public async Task<UserModel?> CreateUser(UserModel user)
  {
    try
    {
      await userCollection.InsertOneAsync(user);
      return user;
    }
    catch
    {
      return null;
    }
  }

  public async Task DeleteUserById(ObjectId id)
  {
    await userCollection.DeleteOneAsync(user => user.Id == id);
  }

  public async Task UpdateUser(UserViewModel user)
  {
    var updateDefinition = Builders<UserModel>.Update
      .Set(u => u.UserDetails, user.UserDetails);
    await userCollection.UpdateOneAsync(u => u.Id == ObjectId.Parse(user.Id), updateDefinition);
  }

  public async Task<IEnumerable<UserModel>> FindAllUsers()
  {
    return await (await userCollection.FindAsync(_ => true)).ToListAsync();
  }

  public async Task<UserModel?> FindUserById(ObjectId id)
  {
    return await (await userCollection.FindAsync(user => user.Id == id)).FirstOrDefaultAsync();
  }

  public async Task<UserModel?> FindUserByUsername(string username)
  {
    return await (await userCollection.FindAsync(user => user.UserDetails.Username == username)).FirstOrDefaultAsync();
  }

  public Task UpdateUser(UserModel user)
  {
    throw new NotImplementedException();
  }
}