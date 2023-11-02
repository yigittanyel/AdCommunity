using AdCommunity.Application.DTOs.User;
using AdCommunity.Domain.Entities.Base;

namespace AdCommunity.Application.Services;

public interface IAuthenticateService
{
    Task<Tokens> Login(UserLoginDto users);
    Task<Tokens> Register(UserCreateDto userData);
}