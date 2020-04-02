using System;
using System.Collections.Generic;
using System.Text;

namespace Medobia.Application.Common.Models
{
  public class UserToken
  {
    public string NameIdentifier { get; set; }
    public string Email { get; set; }
    public string Role { get; set; }
    public string Name { get; set; }
    public string AccessToken { get; set; }
    public string RefreshToken { get; set; }
  }
}
