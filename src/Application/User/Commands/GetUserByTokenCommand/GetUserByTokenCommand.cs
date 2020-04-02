using MediatR;
using Medobia.Application.Common.Interfaces;
using Medobia.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Medobia.Application.User.Commands.GetUserFromAccessToken
{
  public class GetUserByTokenCommand : IRequest<UserToken>
  {
    public string AccessToken { get; set; }

    public class GetUserByTokenCommandHandler : IRequestHandler<GetUserByTokenCommand, UserToken>
    {
      private readonly ITokenService _tokenService;
      public GetUserByTokenCommandHandler(ITokenService tokenService)
      {
        _tokenService = tokenService;
      }

      public async Task<UserToken> Handle(GetUserByTokenCommand request, CancellationToken cancellationToken)
      {
        return await _tokenService.GetUserByTokenAsync(request.AccessToken);
      }
    }
  }
}
