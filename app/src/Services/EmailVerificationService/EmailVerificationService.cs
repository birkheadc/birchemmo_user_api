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
      ),
      TimeSpan.FromHours(24)
    );
  }

  public async Task<bool> ValidateUser(TokenWrapper token)
  {
    try
    {
      TokenData? data = tokenService.ValidateToken(token);
      if (data is null) return false;

      UserModel? user = await userService.GetUserById(data.UserId);
      if (user is null) return false;

      if (user.Id != data.UserId) return false;
      if (user.UserDetails.Role != Role.UNVALIDATED_USER) return false;

      user.UserDetails.Role = Role.VALIDATED_USER;
      await userService.UpdateUserDetails(user.Id, user.UserDetails);
      return true;
    }
    catch
    {
      return false;
    }
  }
}