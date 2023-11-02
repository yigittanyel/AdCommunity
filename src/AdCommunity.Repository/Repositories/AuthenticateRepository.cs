using AdCommunity.Domain.Entities.Base;
using AdCommunity.Domain.Entities.UserModels;
using AdCommunity.Repository.Contracts;
using AdCommunity.Repository.DTOs.User;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AdCommunity.Repository.Repositories;

public class AuthenticateRepository : IAuthenticateRepository
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IConfiguration _configuration;

    public AuthenticateRepository(IUnitOfWork unitOfWork, IConfiguration configuration)
    {
        _unitOfWork = unitOfWork;
        _configuration = configuration;
    }

    public async Task<Tokens> Login(UserLoginDto user)
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

    public async Task<Tokens> Register(UserCreateDto userData)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(userData.Username, userData.Password);

        if (existingUser != null && existingUser.Any())
        {
            return null;
        }

        var user = new User(userData.FirstName, userData.LastName, userData.Email, userData.Password
            , userData.Phone, userData.Username, userData.Website, userData.Facebook, userData.Twitter, userData.Instagram,
            userData.Github, userData.Medium, userData.CreatedOn= DateTime.Now.ToUniversalTime());


        await _unitOfWork.UserRepository.AddAsync(user);
        await _unitOfWork.SaveChangesAsync();

        return await Login(new UserLoginDto { Username = userData.Username, Password = userData.Password });
    }
}
