using MediatR;
using Medobia.Application.Common.Interfaces;
using Medobia.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Medobia.Application.User.Commands.Login
{
  public class LoginCommand : IRequest<UserToken>
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }

    public class LoginCommandHandler : IRequestHandler<LoginCommand, UserToken>
    {
      private readonly IIdentityService _identityService;
      private readonly IApplicationDbContext _context;
      private readonly ITokenService _tokenService;

      public LoginCommandHandler(IIdentityService identityService,
        IApplicationDbContext context,
        ITokenService tokenService)
      {
        _identityService = identityService;
        _context = context;
        _tokenService = tokenService;
      }

      public async Task<UserToken> Handle(LoginCommand request, CancellationToken cancellationToken)
      {
        var result = await _identityService.LoginAsync(request.Email, request.Password, request.RememberMe);

        if (result.Succeeded)
        {
          var user = await _identityService.GetUserResourceAsync(request.Email);
          var response = new UserToken();

          response.NameIdentifier = user.NameIdentifier;
          response.Name = user.Name;
          response.Email = user.Email;
          response.Role = user.Role;

          var refreshToken = _tokenService.GenerateRefreshToken(user.NameIdentifier);

          _context.RefreshTokens.Add(refreshToken);
          await _context.SaveChangesAsync(cancellationToken);

          response.RefreshToken = refreshToken.Token;
          response.AccessToken = _tokenService.GenerateAccessToken(user.NameIdentifier);

          return response;
        }
        else
          return new UserToken();
      }
    }

  }
}
