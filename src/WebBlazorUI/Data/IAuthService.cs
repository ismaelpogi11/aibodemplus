using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebBlazorUI.Commands.Auth;

namespace WebBlazorUI.Data
{
  public interface IAuthService
  {
    Task<CurrentUserResource> LoginAsync(LoginCommand command);
  }
}
