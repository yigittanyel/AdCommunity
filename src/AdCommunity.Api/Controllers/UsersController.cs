using AdCommunity.Domain.Entities.UserModels;
using AdCommunity.Repository.Contracts;
using AdCommunity.Repository.DTOs.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AdCommunity.Api.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class UsersController : ControllerBase
{
    private readonly IAuthenticateRepository _authRepository;

    public UsersController(IAuthenticateRepository authRepository)
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
    public async Task<IActionResult> Register(UserCreateDto usersdata)
    {
        var token = await _authRepository.Register(usersdata);

        if (token == null)
        {
            return Unauthorized();
        }

        return Ok(token);
    }
}