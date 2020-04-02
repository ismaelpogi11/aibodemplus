using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlazorUI.Commands.Auth
{
  public class RefreshTokenCommand
  {
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
  }
}
