using MediatR;
using Medobia.Application.Common.Interfaces;
using Medobia.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Medobia.Application.User.Commands.Register
{
  public class RegisterCommand : IRequest<UserToken>
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }

    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, UserToken>
    {
      private readonly IIdentityService _identityService;
      private readonly ITokenService _tokenService;
      private readonly IApplicationDbContext _context;
      public RegisterCommandHandler(IIdentityService identityService,
        ITokenService tokenService,
        IApplicationDbContext context)
      {
        _identityService = identityService;
        _tokenService = tokenService;
        _context = context;
      }

      public async Task<UserToken> Handle(RegisterCommand request, CancellationToken cancellationToken)
      {
        var userId = await _identityService.CreateUserAsync(request.Email, request.Password);

        if (!string.IsNullOrEmpty(userId))
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

          // Todo : Send confirmation email.

          return response;
        }
        else
          return new UserToken();

      }
    }
  }
}
