using System.Threading.Tasks;
using Medobia.Application.Common.Models;
using Medobia.Application.User.Commands.GetUserFromAccessToken;
using Medobia.Application.User.Commands.Login;
using Medobia.Application.User.Commands.RefreshToken;
using Medobia.Application.User.Commands.Register;
using Microsoft.AspNetCore.Mvc;

namespace WebAPI.Controllers
{
  public class AuthController : ApiController
  {
    [HttpPost]
    [Route("Login")]
    public async Task<ActionResult<UserToken>> Login([FromBody] LoginCommand command)
    {
      return await Mediator.Send(command);
    }

    [HttpPost]
    [Route("Register")]
    public async Task<ActionResult<UserToken>> Register([FromBody] RegisterCommand command)
    {
      return await Mediator.Send(command);
    }

    [HttpPost]
    [Route("RefreshToken")]
    public async Task<ActionResult<UserToken>> RefreshToken([FromBody] RefreshTokenCommand command)
    {
      return await Mediator.Send(command);
    }

    [HttpPost]
    [Route("GetUserByToken")]
    public async Task<ActionResult<UserToken>> GetUserByToken([FromBody] GetUserByTokenCommand command)
    {
      return await Mediator.Send(command);
    }
  }
}