using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebBlazorUI.Commands.Auth
{
  public class LoginCommand
  {
    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
  }
}
