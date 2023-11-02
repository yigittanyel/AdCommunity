using AdCommunity.Domain.Entities.Base;
using AdCommunity.Repository.DTOs.User;

namespace AdCommunity.Repository.Contracts;

public interface IAuthenticateRepository
{
    Task<Tokens> Login(UserLoginDto users);
    Task<Tokens> Register(UserCreateDto userData);
}