using System;
using System.Collections.Generic;
using System.Text;

namespace Medobia.Application.Common.Models
{
  public class UserResource
  {
    public string NameIdentifier { get; set; }
    public string Email { get; set; }
    public string Role { get; set; } = "Guest";
    public string Name { get; set; }
  }
}
