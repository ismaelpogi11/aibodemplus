using System;
using System.Collections.Generic;
using System.Text;

namespace Medobia.Application.Common.Exceptions
{
  public class ExpiredTokenException : Exception
  {
    public ExpiredTokenException()
    : base("Token expired.")
    {
    }
  }
}
