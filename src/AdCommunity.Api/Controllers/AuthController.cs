using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.Authenticate.Commands.LoginCommand;
using AdCommunity.Application.Features.Authenticate.Commands.RegisterCommand;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.SharedKernel;
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
        public async Task<Tokens> Login([FromBody] UserLoginDto user,CancellationToken cancellationToken)
        {
            LoginCommand command = new LoginCommand { User = user };
            var tokens = await _mediator.Send(command, cancellationToken);

            return tokens;
        }

        [HttpPost("register")]
        public async Task<Tokens> Register([FromBody] UserCreateDto user, CancellationToken cancellationToken)
        {
            RegisterCommand command = new RegisterCommand { User = user };
            var createdUser = await _mediator.Send(command, cancellationToken);

            return createdUser;
        }
    }
}
