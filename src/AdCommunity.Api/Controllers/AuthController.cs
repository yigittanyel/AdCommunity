using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.Authenticate.Commands;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public AuthController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto user)
        {
            LoginCommand command = new LoginCommand { User = user };
            var tokens = await _mediator.Send(command);

            if (tokens == null)
            {
                return Unauthorized("Invalid username or password");
            }

            return Ok(tokens);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserCreateDto user)
        {
            RegisterCommand command = new RegisterCommand { User = user };
            var createdUser = await _mediator.Send(command);

            if (createdUser == null)
            {
                return BadRequest();
            }

            return Ok(createdUser);
        }
    }
}
