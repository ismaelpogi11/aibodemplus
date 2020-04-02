using Medobia.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Medobia.Application.Common.Interfaces
{
  public interface IApplicationDbContext
  {
    DbSet<TodoList> TodoLists { get; set; }

    DbSet<TodoItem> TodoItems { get; set; }

    DbSet<RefreshToken> RefreshTokens { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
  }
}
