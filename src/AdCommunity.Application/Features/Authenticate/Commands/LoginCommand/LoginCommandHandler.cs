using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Resources;
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
    private readonly LocalizationService _localizationService;

    public LoginCommandHandler(IConfiguration configuration, IUnitOfWork unitOfWork, LocalizationService localizationService)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
    }

    public async Task<Tokens> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(request.User.Username, request.User.Password);

        if (existingUser is null)
        {
            var errorMessage = _localizationService.GetKey("InvalidCredentialsErrorMessage").Value;
            throw new InvalidCredentialsException(errorMessage);
        }

        var tokenService = new JwtService(_configuration);
        return new Tokens { Token = tokenService.GenerateToken(request.User) };
    }
}