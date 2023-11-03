using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Features.User.Queries;
using AdCommunity.Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAuthenticateService _authRepository;

    public UsersController(IAuthenticateService authRepository)
    {
        _authRepository = authRepository;
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login(UserLoginDto usersdata)
    {
        var token = await _authRepository.Login(usersdata);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }

    [AllowAnonymous]
    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromBody] UserCreateDto usersdata, CancellationToken cancellationToken)
    {
        var token = await _authRepository.Register(usersdata, cancellationToken);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }

}

