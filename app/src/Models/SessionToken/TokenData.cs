namespace BircheMmoUserApi.Models;

using MongoDB.Bson;

public record TokenData
{
  public ObjectId UserId { get; set; }
  public TokenType TokenType { get; set; }
  public TokenData(ObjectId userId, TokenType tokenType)
  {
    UserId = userId;
    TokenType = tokenType;
  }
}