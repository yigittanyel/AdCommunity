using AdCommunity.Application.DTOs.User;
using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Entities.Base;
using AdCommunity.Domain.Repository;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdCommunity.Application.Services;

public class AuthenticateService : IAuthenticateService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthenticateService(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<Tokens> Login(Application.DTOs.User.UserLoginDto user) 
    {
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(user.Username, user.Password);

        if (existingUser == null || !existingUser.Any())
        {
            return null;
        }

        var tokenHandler = new JwtSecurityTokenHandler();
        var tokenKey = Encoding.UTF8.GetBytes(_configuration["JWT:Key"]);
        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.Name, user.Username)
            }),
            Expires = DateTime.UtcNow.AddMinutes(10),
            SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(tokenKey), SecurityAlgorithms.HmacSha256Signature)
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);
        return new Tokens { Token = tokenHandler.WriteToken(token) };
    }

    public async Task<Tokens> Register(UserCreateDto userData,CancellationToken? cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(userData.Username, userData.Password);

        if (existingUser != null && existingUser.Any())
        {
            return null;
        }

        var user = new User(userData.FirstName, userData.LastName, userData.Email, userData.Username
            , userData.Username, userData.Username, userData.Username, userData.Username, userData.Username, userData.Username,
            userData.Username, userData.Username);


        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return await Login(new UserLoginDto(userData.Username,userData.Password));
    }

}
