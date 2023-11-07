using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.User.Commands;
using AdCommunity.Application.Features.User.Queries;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.Aggregates.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers;

[Authorize]
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
    public async Task<UserDto> Get(int userId)
    {
        GetUserQuery query = new GetUserQuery { Id = userId };
        UserDto user = await _mediator.Send(query);
        return user;
    }

    [HttpGet("[action]")]
    public async Task<IEnumerable<UserDto>> GetAll()
    {
        GetUsersQuery query = new GetUsersQuery();
        IEnumerable<UserDto> users = await _mediator.Send(query);

        return users;
    }

    [HttpPost("[action]")]
    public async Task<UserCreateDto> Create(UserCreateDto user)
    {
        CreateUserCommand command = new CreateUserCommand(user.FirstName,user.Password,user.LastName,user.Email,user.Phone,user.Username,user.Website,user.Facebook,user.Twitter,user.Instagram,user.Github,user.Medium);
        UserCreateDto createdUser = await _mediator.Send(command);

        return createdUser;
    }

    [HttpPut("[action]")]
    public async Task<bool> Update(UserUpdateDto user)
    {
        UpdateUserCommand command = new UpdateUserCommand(user.Id,user.FirstName, user.Password, user.LastName, user.Email, user.Phone, user.Username, user.Website, user.Facebook, user.Twitter, user.Instagram, user.Github, user.Medium);
        bool updatedUser = await _mediator.Send(command);

        return updatedUser;
    }

    [HttpDelete("[action]/{userId}")]
    public async Task<bool> Delete(int userId)
    {
        DeleteUserCommand command = new DeleteUserCommand { Id = userId };
        bool deletedUser = await _mediator.Send(command);

        return deletedUser;
    }
}

