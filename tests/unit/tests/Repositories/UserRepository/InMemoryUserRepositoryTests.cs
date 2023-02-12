using System.Collections.Generic;
using System.Threading.Tasks;
using BircheMmoUserApi.Models;
using BircheMmoUserApi.Repositories;
using BircheMmoUserApiUnitTests.Mocks.Builders;
using MongoDB.Bson;
using Xunit;

namespace BircheMmoUserApiUnitTests.Repositories;

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

    UserModel newUser = new MockUserModelBuilder()
      .Build();

    UserModel? model = await repository.CreateUser(newUser);
    Assert.NotNull(model);

    model = await repository.CreateUser(newUser);
    Assert.Null(model);
  }

  [Fact]
  public async Task CreateUser_Does_Not_Create_If_Username_Clash()
  {
    InMemoryUserRepository repository = new();

    UserModel newUser = new MockUserModelBuilder()
      .Build();

    await repository.CreateUser(newUser);
    await repository.CreateUser(newUser);
    
    List<UserModel> users = new();
    users.AddRange(await repository.FindAllUsers());
    Assert.True(users.Count == 1);
  }

  [Fact]
  public async Task CreateUser_Does_Not_Create_If_EmailAddress_Clash()
  {
    InMemoryUserRepository repository = new();

    string emailAddress = "repeat@site.com";

    UserModel newUser = new MockUserModelBuilder()
      .WithEmailAddress(emailAddress)
      .Build();

    UserModel repeatUser = new MockUserModelBuilder()
      .WithEmailAddress(emailAddress)
      .Build();

    await repository.CreateUser(newUser);
    await repository.CreateUser(repeatUser);
    
    List<UserModel> users = new();
    users.AddRange(await repository.FindAllUsers());
    Assert.True(users.Count == 1);
  }

  [Fact]
  public async Task CreateUser_FindUserById_Returns_Created_User()
  {
    InMemoryUserRepository repository = new();

    UserModel newUser = new MockUserModelBuilder()
      .Build();

    ObjectId id = newUser.Id;

    await repository.CreateUser(newUser);

    UserModel? user = await repository.FindUserById(id);
    Assert.NotNull(user);
    Assert.Equal(newUser.UserDetails.Username, user.UserDetails.Username);
  }

  [Theory]
  [InlineData("oldcheddar", Role.ADMIN)]
  [InlineData("newcheddar", Role.UNVALIDATED_USER)]
  public async Task CreateUser_FindUserByUsername_Returns_Created_User(string username, Role role)
  {
    InMemoryUserRepository repository = new();

    UserModel newUser = new MockUserModelBuilder()
      .WithUsername(username)
      .WithRole(role)
      .Build();

    await repository.CreateUser(newUser);

    UserModel? user = await repository.FindUserByUsername(username);
    Assert.NotNull(user);
    Assert.Equal(newUser.UserDetails.Username, user.UserDetails.Username);
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
        users.Find(user => user.UserDetails.Username == newUsers[i].UserDetails.Username)
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
  public async Task UpdateUserDetails_Updates_User_Details()
  {
    InMemoryUserRepository repository = new();
    
    UserModel newUser = new MockUserModelBuilder()
      .WithUsername("old")
      .WithRole(Role.ADMIN)
      .Build();

    ObjectId id = newUser.Id;

    await repository.CreateUser(newUser);

    UserModel? userModel = await repository.FindUserById(id);

    Assert.NotNull(userModel);
    Assert.Equal("old", userModel.UserDetails.Username);
    Assert.Equal(Role.ADMIN, userModel.UserDetails.Role);

    UserDetails updatedUserDetails = CopyUserDetails(newUser.UserDetails);
    updatedUserDetails.Username = "new";
    updatedUserDetails.Role = Role.UNVALIDATED_USER;

    await repository.UpdateUserDetails(id, updatedUserDetails);

    userModel = await repository.FindUserByUsername("old");
    Assert.Null(userModel);

    userModel = await repository.FindUserById(id);
    Assert.NotNull(userModel);
    Assert.Equal("new", userModel.UserDetails.Username);
    Assert.Equal(Role.UNVALIDATED_USER, userModel.UserDetails.Role);
  }

  [Fact]
  public async Task UpdateUserDetails_Does_Nothing_If_User_Not_Found()
  {
    InMemoryUserRepository repository = new();
    
    UserModel newUser = new MockUserModelBuilder()
      .WithUsername("old")
      .Build();

    await repository.CreateUser(newUser);

    UserModel? user = await repository.FindUserByUsername("old");
    Assert.NotNull(user);

    UserDetails updatedUserDetails = CopyUserDetails(newUser.UserDetails);
    updatedUserDetails.Username = "new";
    
    await repository.UpdateUserDetails(ObjectId.GenerateNewId(), updatedUserDetails);

    user = await repository.FindUserByUsername("old");
    Assert.NotNull(user);

    user = await repository.FindUserByUsername("new");
    Assert.Null(user);
  }

  [Fact]
  public async Task UpdatePassword_Updates_Password()
  {
    InMemoryUserRepository repository = new();
    
    UserModel newUser = new MockUserModelBuilder()
      .WithUsername("old")
      .WithPassword("oldpassword")
      .Build();

    Assert.True(BCrypt.Net.BCrypt.Verify("oldpassword", newUser.HashedPassword));

    await repository.CreateUser(newUser);

    UserModel? user = await repository.FindUserByUsername("old");
    Assert.NotNull(user);
    
    await repository.UpdatePassword(newUser.Id, "newpassword");

    user = await repository.FindUserByUsername("old");
    Assert.NotNull(user);

    Assert.False(BCrypt.Net.BCrypt.Verify("oldpassword", newUser.HashedPassword));
    Assert.True(BCrypt.Net.BCrypt.Verify("newpassword", newUser.HashedPassword));
  }

  [Fact]
  public async Task UpdatePassword_Does_Nothing_If_User_Not_Found()
  {
    InMemoryUserRepository repository = new();
    
    UserModel newUser = new MockUserModelBuilder()
      .WithUsername("old")
      .WithPassword("oldpassword")
      .Build();

    Assert.True(BCrypt.Net.BCrypt.Verify("oldpassword", newUser.HashedPassword));

    await repository.CreateUser(newUser);

    UserModel? user = await repository.FindUserByUsername("old");
    Assert.NotNull(user);
    
    await repository.UpdatePassword(ObjectId.GenerateNewId(), "newpassword");

    user = await repository.FindUserByUsername("old");
    Assert.NotNull(user);

    Assert.True(BCrypt.Net.BCrypt.Verify("oldpassword", newUser.HashedPassword));
    Assert.False(BCrypt.Net.BCrypt.Verify("newpassword", newUser.HashedPassword));
  }

  private List<UserModel> GenerateListOfNUserModels(int n)
  {
    List<UserModel> users = new();
    for (int i = 0; i < n; i++)
    {
      users.Add(
        new MockUserModelBuilder().Build()
      );
    }
    return users;
  }

  private UserDetails CopyUserDetails(UserDetails original)
  {
    return new UserDetails(
      original.Username,
      original.EmailAddress,
      original.Role,
      original.SendMeUpdates
    );
  }
}