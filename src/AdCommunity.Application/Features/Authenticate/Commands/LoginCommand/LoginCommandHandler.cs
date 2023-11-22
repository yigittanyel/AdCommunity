using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Jwt;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.SharedKernel;
using AdCommunity.Domain.Repository;
using Microsoft.Extensions.Configuration;

namespace AdCommunity.Application.Features.Authenticate.Commands.LoginCommand;

public class LoginCommandHandler : IYtRequestHandler<LoginCommand, Tokens>
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;

    public LoginCommandHandler(IConfiguration configuration, IUnitOfWork unitOfWork)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
    }

    public async Task<Tokens> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(request.User.Username, request.User.Password);

        if (existingUser is null)
        {
            throw new InvalidCredentialsException();
        }

        var tokenService = new JwtService(_configuration);
        return new Tokens { Token = tokenService.GenerateToken(request.User) };
    }
}