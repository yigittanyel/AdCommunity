using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.User.Queries;
using AdCommunity.Core.CustomMediator;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly IYtMediator _mediator;

        public ValuesController(IYtMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("[action]/{userId}")]
        public async Task<IActionResult> Get(int userId)
        {
            GetUserQuery query = new GetUserQuery { Id = userId };
            UserDto user = await _mediator.Send(query);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }
    }
}
