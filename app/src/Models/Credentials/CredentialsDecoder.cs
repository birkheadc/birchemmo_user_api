using System.Text;

namespace BircheMmoUserApi.Models;

public class CredentialsDecoder
{
  public Credentials? DecodeCredentialsFromBasic(string basic)
  {
    try
    {
      string encodedCredentials = basic.Substring("Basic ".Length).Trim();

      Encoding encoding = Encoding.GetEncoding("iso-8859-1");
      string decodedCredentials = encoding.GetString(Convert.FromBase64String(encodedCredentials));

      int i = decodedCredentials.IndexOf(':');

      return new Credentials(
        decodedCredentials.Substring(0, i),
        decodedCredentials.Substring(i + 1)
      );
    }
    catch
    {
      return null;
    }
  }
}