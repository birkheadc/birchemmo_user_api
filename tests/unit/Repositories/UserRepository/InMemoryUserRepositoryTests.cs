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
  public async Task CreateUser_Returns_Null_If_Username_Clash()
  {
    InMemoryUserRepository repository = new();

    UserModel newUser = new(
      ObjectId.GenerateNewId(),
      "oldcheddar",
      "password",
      Role.USER,
      false
    );

    UserModel? model = await repository.CreateUser(newUser);
    Assert.NotNull(model);

    model = await repository.CreateUser(newUser);
    Assert.Null(model);
  }

  [Fact]
  public async Task CreateUser_Does_Not_Create_If_Username_Clash()
  {
    InMemoryUserRepository repository = new();

    UserModel newUser = new(
      ObjectId.GenerateNewId(),
      "oldcheddar",
      "password",
      Role.USER,
      false
    );

    await repository.CreateUser(newUser);
    await repository.CreateUser(newUser);
    
    List<UserModel> users = new();
    users.AddRange(await repository.FindAllUsers());
    Assert.True(users.Count == 1);
  }

  [Fact]
  public async Task CreateUser_FindUserById_Returns_Created_User()
  {
    InMemoryUserRepository repository = new();

    ObjectId id = ObjectId.GenerateNewId();
    UserModel newUser = new(
      id,
      "oldcheddar",
      "passw0rd",
      Role.ADMIN,
      false
    );

    await repository.CreateUser(newUser);

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

    UserModel newUser = new(
      ObjectId.GenerateNewId(),
      username,
      "passw0rd",
      role,
      false
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
    List<UserModel> newUsers = GenerateListOfNUserModels(numUsers);

    InMemoryUserRepository repository = new();

    foreach (UserModel newUser in newUsers)
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
    InMemoryUserRepository repository = new();

    List<UserModel> newUsers = GenerateListOfNUserModels(3);

    foreach (UserModel newUser in newUsers)
    {
      await repository.CreateUser(newUser);
    }

    List<UserModel> users = new();
    users.AddRange(await repository.FindAllUsers());

    Assert.True(users.Count == 3);

    await repository.DeleteUserById(users[0].Id);

    users = new();
    users.AddRange(await repository.FindAllUsers());

    Assert.True(users.Count == 2);
  }

  [Fact]
  public async Task DeleteUserByID_Does_Nothing_If_Id_Not_Found()
  {
    InMemoryUserRepository repository = new();

    List<UserModel> newUsers = GenerateListOfNUserModels(3);

    foreach (UserModel newUser in newUsers)
    {
      await repository.CreateUser(newUser);
    }

    List<UserModel> users = new();
    users.AddRange(await repository.FindAllUsers());

    Assert.True(users.Count == 3);

    await repository.DeleteUserById(ObjectId.Empty);

    users = new();
    users.AddRange(await repository.FindAllUsers());

    Assert.True(users.Count == 3);
  }

  [Fact]
  public async Task EditUser_Updates_Values()
  {
    InMemoryUserRepository repository = new();
    
    ObjectId id = ObjectId.GenerateNewId();
    UserModel newUser = new(
      id,
      "oldcheddar",
      "passw0rd",
      Role.ADMIN,
      false
    );

    await repository.CreateUser(newUser);

    UserModel? userModel = await repository.FindUserById(id);

    Assert.NotNull(userModel);
    Assert.Equal(userModel.Username, "oldcheddar");
    Assert.Equal(userModel.Role, Role.ADMIN);

    UserViewModel editUser = new(
      id.ToString(),
      "newcheddar",
      Role.USER,
      false
    );
    await repository.EditUser(editUser);

    userModel = await repository.FindUserByUsername("oldcheddar");
    Assert.Null(userModel);

    userModel = await repository.FindUserById(id);
    Assert.NotNull(userModel);
    Assert.Equal(userModel.Username, "newcheddar");
    Assert.Equal(userModel.Role, Role.USER);
  }

  [Fact]
  public async Task EditUser_Does_Nothing_If_User_Not_Found()
  {
    InMemoryUserRepository repository = new();
    
    UserModel newUser = new(
      ObjectId.GenerateNewId(),
      "oldcheddar",
      "passw0rd",
      Role.ADMIN,
      false
    );

    await repository.CreateUser(newUser);

    UserViewModel editUser = new(
      ObjectId.Empty.ToString(),
      "newcheddar",
      Role.USER,
      false
    );
    await repository.EditUser(editUser);

    UserModel? user = await repository.FindUserByUsername("oldcheddar");
    Assert.NotNull(user);
  }

  private List<UserModel> GenerateListOfNUserModels(int n)
  {
    List<UserModel> users = new();
    for (int i = 0; i < n; i++)
    {
      users.Add(new(
        ObjectId.GenerateNewId(),
        "user_" + i.ToString(),
        "passw0rd",
        Role.USER,
        false
      ));
    }
    return users;
  }
}