using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace WebBlazorUI.Data
{
  public class AuthStateProvider : AuthenticationStateProvider
  {
    private readonly ILocalStorageService _localStorageService;
    private readonly IAuthService _authService;

    public AuthStateProvider(ILocalStorageService localStorageService,
        IAuthService authService)
    {
      _localStorageService = localStorageService;
      _authService = authService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
      var accessToken = await _localStorageService.GetItemAsync<string>("accessToken");

      ClaimsIdentity identity;

      if (accessToken != null && accessToken != string.Empty)
      {
        CurrentUserResource user = await _authService.GetUserByAccessTokenAsync(accessToken);
        identity = GetClaimsIdentity(user);
      }
      else
      {
        identity = new ClaimsIdentity();
      }

      var claimsPrincipal = new ClaimsPrincipal(identity);

      return await Task.FromResult(new AuthenticationState(claimsPrincipal));
    }

    public async Task SetUserAsAuthenticated(CurrentUserResource user)
    {
      await _localStorageService.SetItemAsync("accessToken", user.AccessToken);
      await _localStorageService.SetItemAsync("refreshToken", user.RefreshToken);

      var identity = GetClaimsIdentity(user);

      var claimsPrincipal = new ClaimsPrincipal(identity);

      NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(claimsPrincipal)));
    }

    public async Task SetUserAsLoggedOut()
    {
      await _localStorageService.RemoveItemAsync("refreshToken");
      await _localStorageService.RemoveItemAsync("accessToken");

      var identity = new ClaimsIdentity();

      var user = new ClaimsPrincipal(identity);

      NotifyAuthenticationStateChanged(Task.FromResult(new AuthenticationState(user)));
    }

    private ClaimsIdentity GetClaimsIdentity(CurrentUserResource user)
    {
      var claimsIdentity = new ClaimsIdentity();

      if (user.NameIdentifier != null)
      {
        claimsIdentity = new ClaimsIdentity(new[]
                          {
                              new Claim(ClaimTypes.NameIdentifier, user.NameIdentifier),
                              new Claim(ClaimTypes.Name, user.Name),
                              new Claim(ClaimTypes.Email, user.Email),
                              new Claim(ClaimTypes.Role, user.Role),
                          }, "apiauth_type");
      }

      return claimsIdentity;
    }

  }
}
