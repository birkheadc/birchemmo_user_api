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
    if (IsUserValid(user) == false) {
      return null;
    }
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

   public async Task UpdateUserDetails(ObjectId id, UserDetails userDetails)
  {
    FilterDefinition<UserModel> filter = Builders<UserModel>.Filter.Where(user => user.Id == id);
    UpdateDefinition<UserModel> update = Builders<UserModel>.Update.Set(user => user.UserDetails, userDetails);
    await userCollection.FindOneAndUpdateAsync(filter, update);
  }

  public Task UpdatePassword(ObjectId id, string newPassword)
  {
    throw new NotImplementedException();
  }

  public async Task<UserModel?> FindUserByEmailAddress(string emailAddress)
  {
    return await (await userCollection.FindAsync(user => user.UserDetails.EmailAddress == emailAddress)).FirstOrDefaultAsync();
  }

  private bool IsUserValid(UserModel user)
  {
    if (DoesUserWithSameUsernameOrEmailAddressExist(user.UserDetails) == true) return false;
    return true;
  }

  private bool DoesUserWithSameUsernameOrEmailAddressExist(UserDetails userDetails)
  {
    FilterDefinition<UserModel> filter = Builders<UserModel>.Filter
      .Where(_ => _.UserDetails.EmailAddress == userDetails.EmailAddress || _.UserDetails.Username == userDetails.Username);
    return userCollection.Find(filter).Any();
  }
}