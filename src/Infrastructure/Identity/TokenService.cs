using Medobia.Application.Common.Interfaces;
using Medobia.Application.Common.Models;
using Medobia.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Medobia.Infrastructure.Identity
{
  public class TokenService : ITokenService
  {
    //private const string SecretKey = "Medob1@_by_!sm2el3n@jE_07232019_d65ade02adfe4f059e686c9d9e7c7bac";

    private const int TOKEN_LIFE = 10;
    private const int REFRESH_LIFE = 20;

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public TokenService(IApplicationDbContext context,
      UserManager<ApplicationUser> userManager,
      IConfiguration configuration)
    {
      _context = context;
      _userManager = userManager;
      _configuration = configuration;
    }

    public RefreshToken GenerateRefreshToken(string userId)
    {
      var refreshToken = new RefreshToken();
      refreshToken.UserId = userId;

      var randomNumber = new byte[32];
      using (var rng = RandomNumberGenerator.Create())
      {
        rng.GetBytes(randomNumber);
        refreshToken.Token = Convert.ToBase64String(randomNumber);
      }
      refreshToken.ExpiryDate = DateTime.UtcNow.AddSeconds(REFRESH_LIFE);

      return refreshToken;
    }

    public string GenerateAccessToken(string userId)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.ASCII.GetBytes(_configuration["Tokens:Key"]);
      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = new ClaimsIdentity(new Claim[]
        {
            new Claim(ClaimTypes.NameIdentifier, userId)
        }),
        Expires = DateTime.UtcNow.AddSeconds(TOKEN_LIFE),
        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
          SecurityAlgorithms.HmacSha256Signature)
      };
      var token = tokenHandler.CreateToken(tokenDescriptor);

      return tokenHandler.WriteToken(token);
    }

    public bool ValidateRefreshToken(ApplicationUser user, string refreshToken)
    {
      var refreshTokenUser = _context.RefreshTokens.Where(rt => rt.Token == refreshToken)
                                          .OrderByDescending(rt => rt.ExpiryDate)
                                          .FirstOrDefault();

      if (refreshTokenUser != null && refreshTokenUser.UserId == user.Id
          && refreshTokenUser.ExpiryDate > DateTime.UtcNow)
      {
        return true;
      }

      return false;
    }

    public async Task<UserToken> RefreshTokenAsync(string accessToken, string refreshToken)
    {
      var user = await GetUserFromAccessTokenAsync(accessToken);

      if (user != null && ValidateRefreshToken(user, refreshToken))
      {
        var userWithToken = new UserToken();
        var role = await _userManager.GetRolesAsync(user);

        userWithToken.Email = user.Email;
        userWithToken.Name = user.UserName;
        userWithToken.NameIdentifier = user.Id;
        userWithToken.Role = role.FirstOrDefault() ?? "Guest";

        var newRefreshToken = GenerateRefreshToken(user.Id);

        _context.RefreshTokens.Add(newRefreshToken);
        await _context.SaveChangesAsync(new System.Threading.CancellationToken());

        userWithToken.RefreshToken = newRefreshToken.Token;
        userWithToken.AccessToken = GenerateAccessToken(user.Id);

        return userWithToken;
      }

      return null;
    }

    public async Task<UserToken> GetUserByTokenAsync(string accessToken)
    {
      var user = await GetUserFromAccessTokenAsync(accessToken);

      if (user != null)
      {
        var userWithToken = new UserToken();
        var role = await _userManager.GetRolesAsync(user);

        userWithToken.Email = user.Email;
        userWithToken.Name = user.UserName;
        userWithToken.NameIdentifier = user.Id;
        userWithToken.Role = role.FirstOrDefault();

        return userWithToken;
      }

      return null;
    }

    private async Task<ApplicationUser> GetUserFromAccessTokenAsync(string accessToken)
    {
      try
      {
        var tokenHandler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["Tokens:Key"]);

        var tokenValidationParameters = new TokenValidationParameters
        {
          ValidateIssuerSigningKey = true,
          IssuerSigningKey = new SymmetricSecurityKey(key),
          ValidateIssuer = false,
          ValidateAudience = false
        };

        SecurityToken securityToken;
        var principal = tokenHandler.ValidateToken(accessToken, tokenValidationParameters, out securityToken);

        var jwtSecurityToken = securityToken as JwtSecurityToken;
        if (jwtSecurityToken != null && jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
        {
          var userId = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
          return await _userManager.FindByIdAsync(userId);
        }
      }
      catch (Exception)
      {
        return new ApplicationUser();
      }

      return new ApplicationUser();
    }

  }
}
