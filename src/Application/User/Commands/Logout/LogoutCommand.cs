using MediatR;
using Medobia.Application.Common.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Medobia.Application.User.Commands.Logout
{
  public class LogoutCommand : IRequest<Result>
  {
    public class LogoutCommandHandler : IRequestHandler<LogoutCommand, Result>
    {
      private readonly IIdentityService _identityService;
      public LogoutCommandHandler(IIdentityService identityService)
      {
        _identityService = identityService;
      }

      public async Task<Result> Handle(LogoutCommand request, CancellationToken cancellationToken)
      {
        return await _identityService.LogoutAsync();
      }
    }
  }
}
