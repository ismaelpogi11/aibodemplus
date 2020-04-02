using Medobia.Application.Common.Models;
using Medobia.Application.User.Commands.Login;
using System.Threading.Tasks;

namespace Medobia.Application
{
  public interface IIdentityService
  {
    Task<string> GetUserNameAsync(string userId);

    Task<string> CreateUserAsync(string userName, string password);

    Task<Result> DeleteUserAsync(string userId);

    Task<Result> LoginAsync(string email, string password, bool rememberMe);

    Task<Result> LogoutAsync();

    Task<UserResource> GetUserResourceAsync(string email);
  }
}
