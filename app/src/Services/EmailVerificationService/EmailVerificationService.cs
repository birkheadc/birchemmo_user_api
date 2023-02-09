using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using BircheMmoUserApi.Config;
using BircheMmoUserApi.Models;
using Microsoft.IdentityModel.Tokens;

namespace BircheMmoUserApi.Services;

public class EmailVerificationService : IEmailVerificationService
{
  private readonly IUserService userService;
  private readonly IJwtService tokenService;

  public EmailVerificationService(IUserService userService, IJwtService tokenService)
  {
    this.userService = userService;
    this.tokenService = tokenService;
  }

  public async Task<TokenWrapper?> GenerateForUser(UserModel user)
  {
    UserModel? u = await userService.GetUserById(user.Id);
    if (u is null) return null;

    return tokenService.GenerateToken(
      new TokenData(
        user.Id,
        TokenType.EmailVerification
      )
    );
  }

  public async Task<bool> Validate(TokenWrapper token)
  {
    try
    {
      TokenData? data = tokenService.ValidateToken(token);
      if (data is null) return false;

      UserModel? user = await userService.GetUserById(data.UserId);
      if (user is null) return false;

      if (user.Id != data.UserId) return false;
      if (user.IsEmailVerified == true) return false;

      user.IsEmailVerified = true;
      await userService.UpdateUser(user);
      return true;
    }
    catch
    {
      return false;
    }
  }
}