namespace BircheMmoUserApi.ErrorMessages;

public enum UnauthorizedErrorMessage
{
  UNEXPECTED_ERROR,
  MISSING_AUTHORIZATION_HEADER,
  AUTHORIZATION_HEADER_BAD_FORMAT,
  TOKEN_INVALID
}