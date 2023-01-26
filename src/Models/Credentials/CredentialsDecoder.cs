using System.Text;

namespace BircheMmoUserApi.Models;

public static class CredentialsDecoder
{
  public static Credentials DecodeCredentialsFromBasic(string basic)
  {
    string encodedCredentials = basic.Substring("Basic ".Length).Trim();

    Encoding encoding = Encoding.GetEncoding("iso-8859-1");
    string decodedCredentials = encoding.GetString(Convert.FromBase64String(encodedCredentials));

    int i = decodedCredentials.IndexOf(':');

    return new Credentials()
    {
      Username = decodedCredentials.Substring(0, i),
      Password = decodedCredentials.Substring(i + 1)
    };
  }
}