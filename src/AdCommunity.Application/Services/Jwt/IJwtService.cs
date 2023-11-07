using AdCommunity.Application.DTOs.User;

namespace AdCommunity.Application.Services.Jwt;
public interface IJwtService
{
    string GenerateToken(UserLoginDto user);
}