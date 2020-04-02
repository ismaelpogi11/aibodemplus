using IdentityServer4.Services;
using Medobia.Application;
using Medobia.Application.Common.Models;
using Medobia.Application.User.Commands.Login;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

namespace Medobia.Infrastructure.Identity
{
  public class IdentityService : IIdentityService
  {
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;
    private const string GUEST = "Guest";

    public IdentityService(UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
      _userManager = userManager;
      _signInManager = signInManager;
      _tokenService = tokenService;
    }

    public async Task<string> GetUserNameAsync(string userId)
    {
      var user = await _userManager.Users.FirstAsync(u => u.Id == userId);

      return user.UserName;
    }

    public async Task<string> CreateUserAsync(string userName, string password)
    {
      var user = new ApplicationUser
      {
        UserName = userName,
        Email = userName,
      };

      var result = await _userManager.CreateAsync(user, password);
      return user.Id;
    }

    public async Task<Result> DeleteUserAsync(string userId)
    {
      var user = _userManager.Users.SingleOrDefault(u => u.Id == userId);

      if (user != null)
      {
        return await DeleteUserAsync(user);
      }

      return Result.Success();
    }

    public async Task<Result> DeleteUserAsync(ApplicationUser user)
    {
      var result = await _userManager.DeleteAsync(user);

      return result.ToApplicationResult();
    }

    public async Task<Result> LoginAsync(string email, string password, bool rememberMe)
    {
      var signInResult = await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);

      if (signInResult.Succeeded)
        return Result.Success();

      if (signInResult.RequiresTwoFactor)
        return Result.Failure(new string[] { "Two factor authentication required." });

      if (signInResult.IsLockedOut)
        return Result.Failure(new string[] { "User account locked out." });

      return Result.Failure(new string[] { "Invalid login attempt." });
    }

    public async Task<UserResource> GetUserResourceAsync(string email)
    {
      var applicationUser = await _userManager.Users.FirstOrDefaultAsync(u => u.Email == email);
      var userRoles = await _userManager.GetRolesAsync(applicationUser);

      var resource = new UserResource();

      resource.NameIdentifier = applicationUser.Id;
      resource.Email = applicationUser.Email;
      resource.Name = applicationUser.UserName;
      resource.Role = userRoles.FirstOrDefault() ?? GUEST;

      return resource;
    }

    public async Task<Result> LogoutAsync()
    {
      await _signInManager.SignOutAsync();


      return Result.Success();
    }

  }
}
