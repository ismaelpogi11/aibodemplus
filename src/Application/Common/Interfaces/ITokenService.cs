using Medobia.Application.Common.Models;
using Medobia.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Medobia.Application.Common.Interfaces
{
  public interface ITokenService
  {
    RefreshToken GenerateRefreshToken(string userId);
    string GenerateAccessToken(string userId);
    Task<UserToken> RefreshTokenAsync(string accessToken, string refreshToken);
    Task<UserToken> GetUserByTokenAsync(string accessToken);
  }
}
