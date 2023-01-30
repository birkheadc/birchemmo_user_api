using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiTests.Repositories;

public class InMemoryUserRepositoryTests
{
  [Fact]
  public void Repository_Resolves()
  {
    InMemoryUserRepository repository = new();

    Assert.NotNull(repository);
  }

  [Fact]
  public async Task FindAllUsers_Returns_Empty_Set()
  {
    InMemoryUserRepository repository = new();

    List<UserModel> users = new();
    users.AddRange(await repository.FindAllUsers());

    Assert.NotNull(users);
    Assert.Empty(users);
  }

  [Fact]
  public async Task FindUserById_Returns_Null()
  {
    InMemoryUserRepository repository = new();

    UserModel? user = await repository.FindUserById(ObjectId.GenerateNewId());

    Assert.Null(user);
  }

  [Fact]
  public async Task FindUserByUsername_Returns_Null()
  {
    InMemoryUserRepository repository = new();

    UserModel? user = await repository.FindUserByUsername("oldcheddar");

    Assert.Null(user);
  }

  [Fact]
  public async Task CreateUser_FindUserById_Returns_Created_User()
  {
    InMemoryUserRepository repository = new();

    NewUserModel newUser = new(
      "oldcheddar",
      "passw0rd",
      Role.ADMIN
    );

    UserModel? _ = await repository.CreateUser(newUser);
    Assert.NotNull(_);
    ObjectId id = _.Id;

    UserModel? user = await repository.FindUserById(id);
    Assert.NotNull(user);
    Assert.Equal(newUser.Username, user.Username);
  }

  [Theory]
  [InlineData("oldcheddar", Role.ADMIN)]
  [InlineData("newcheddar", Role.USER)]
  public async Task CreateUser_FindUserByUsername_Returns_Created_User(string username, Role role)
  {
    InMemoryUserRepository repository = new();

    NewUserModel newUser = new(
      username,
      "passw0rd",
      role
    );

    await repository.CreateUser(newUser);

    UserModel? user = await repository.FindUserByUsername(username);
    Assert.NotNull(user);
    Assert.Equal(newUser.Username, user.Username);
  }

  [Theory]
  [InlineData(1)]
  [InlineData(2)]
  [InlineData(3)]
  public async Task CreateUser_FindAllUsers_Returns_Set_Of_Those_Users(int numUsers)
  {
    List<NewUserModel> newUsers = new();
    for (int i = 0; i < numUsers; i++)
    {
      newUsers.Add(new(
        "user_" + i.ToString(),
        "passw0rd",
        Role.USER
      ));
    }

    InMemoryUserRepository repository = new();

    foreach (NewUserModel newUser in newUsers)
    {
      await repository.CreateUser(newUser);
    }

    List<UserModel> users = new();
    users.AddRange(await repository.FindAllUsers());

    Assert.NotNull(users);
    Assert.True(users.Count == numUsers);

    for (int i = 0; i < numUsers; i++)
    {
      Assert.NotNull(
        users.Find(user => user.Username == newUsers[i].Username)
      );
    }
  }

  [Fact]
  public async Task DeleteUserById_Removes_User_From_Database()
  {
    // Todo
  }

  [Fact]
  public async Task DeleteUserByID_Does_Nothing_If_Id_Not_Found()
  {
    // Todo
  }

  [Fact]
  public async Task EditUser_Updates_Values()
  {
    // Todo
  }

  [Fact]
  public async Task EditUser_Does_Nothing_If_User_Not_Found()
  {
    // Todo
  }
}