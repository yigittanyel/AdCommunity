using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.User.Commands;
using AdCommunity.Application.Features.User.Queries;
using AdCommunity.Core.CustomMediator.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IYtMediator _mediator;

    public UsersController(IYtMediator mediator)
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

    [HttpGet("[action]")]
    public async Task<IActionResult> GetAll()
    {
        GetUsersQuery query = new GetUsersQuery();
        IEnumerable<UserDto> users = await _mediator.Send(query);

        if (users == null)
        {
            return NotFound();
        }

        return Ok(users);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Create(UserCreateDto user)
    {
        CreateUserCommand command = new CreateUserCommand { User = user };
        UserCreateDto createdUser = await _mediator.Send(command);

        if (createdUser == null)
        {
            return BadRequest();
        }

        return Ok(createdUser);
    }
}

