using System;
using System.Collections.Generic;
using System.Text;

namespace Medobia.Domain.Entities
{
  public class RefreshToken
  {
    public int Id { get; set; }
    public string UserId { get; set; }
    public string Token { get; set; }
    public DateTime ExpiryDate { get; set; }
  }
}
