using BircheMmoUserApi.Models;
using Xunit;

namespace BircheMmoUserApiTests.Models;

public class CredentialsDecoderTests
{
  [Theory]
  [InlineData("Basic dXNlcm5hbWU6cGFzc3dvcmQ=", "username", "password")]
  [InlineData("Basic c2xkYWtqZmxzamlkOnBhc2FzZGZhc2RhZWZhc2QxMjMxMjM0", "sldakjflsjid", "pasasdfasdaefasd1231234")]
  public void DecodeCredentialsFromBasic_Decodes_Value_Correctly(string basic, string expectedUsername, string expectedPassword)
  {
    CredentialsDecoder decoder = new();

    Credentials? credentials = decoder.DecodeCredentialsFromBasic(basic);

    Assert.NotNull(credentials);
    Assert.Equal(credentials.Username, expectedUsername);
    Assert.Equal(credentials.Password, expectedPassword);
  }

  [Theory]
  [InlineData("bad_data")]
  [InlineData("Basicc2xhc2RmYXNkamZsc2ppZDpwYXNzZDEyMzEyMzQ=")]
  [InlineData(" c2xhc2RmYXNkamZsc2ppZDpwYXNzZDEyMzEyMzQ= ")]
  public void DecodeCredentialsFromBasic_Returns_Null_If_Given_Bad_Data(string basic)
  {
    CredentialsDecoder decoder = new();

    Credentials? credentials = decoder.DecodeCredentialsFromBasic(basic);

    Assert.Null(credentials);
  }
}