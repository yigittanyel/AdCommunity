using AdCommunity.Application.DTOs.User;

namespace AdCommunity.Application.Services;
public interface IJwtService
{
    string GenerateToken(UserLoginDto user);
}