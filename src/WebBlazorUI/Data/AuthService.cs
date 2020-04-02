using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using WebBlazorUI.Commands.Auth;

namespace WebBlazorUI.Data
{
  public class AuthService : IAuthService
  {
    public readonly HttpClient _httpClient;
    public AuthService(HttpClient httpClient)
    {
      _httpClient = httpClient;
    }

    public async Task<CurrentUserResource> LoginAsync(LoginCommand command)
    {
      var json = JsonConvert.SerializeObject(command);
      var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Login");

      requestMessage.Content = new StringContent(json);
      requestMessage.Content.Headers.ContentType =
          new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

      var response = await _httpClient.SendAsync(requestMessage);

      var statusCode = response.StatusCode;
      var responseBody = await response.Content.ReadAsStringAsync();

      var userResource = JsonConvert.DeserializeObject<CurrentUserResource>(responseBody);

      return await Task.FromResult(userResource);
    }

    public async Task<CurrentUserResource> RefreshTokenAsync(RefreshTokenCommand command)
    {
      var json = JsonConvert.SerializeObject(command);
      var requestMessage = new HttpRequestMessage(HttpMethod.Post, "Login");

      requestMessage.Content = new StringContent(json);
      requestMessage.Content.Headers.ContentType =
          new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

      var response = await _httpClient.SendAsync(requestMessage);

      var statusCode = response.StatusCode;
      var responseBody = await response.Content.ReadAsStringAsync();

      var userResource = JsonConvert.DeserializeObject<CurrentUserResource>(responseBody);

      return await Task.FromResult(userResource);
    }


  }
}
 