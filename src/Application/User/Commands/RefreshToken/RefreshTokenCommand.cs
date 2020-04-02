using MediatR;
using Medobia.Application.Common.Interfaces;
using Medobia.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Medobia.Application.User.Commands.RefreshToken
{
  public class RefreshTokenCommand : IRequest<UserToken>
  {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }

    public class RefreshTokenCommandHandler : IRequestHandler<RefreshTokenCommand, UserToken>
    {
      private readonly ITokenService _tokenService;
      public RefreshTokenCommandHandler(ITokenService tokenService)
      {
        _tokenService = tokenService;
      }

      public async Task<UserToken> Handle(RefreshTokenCommand request, CancellationToken cancellationToken)
      {
        return await _tokenService.RefreshTokenAsync(request.AccessToken, request.RefreshToken);
      }
    }
  }
}
