using AdCommunity.Application.Services.Jwt;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Domain.Entities.SharedKernel;
using AdCommunity.Repository.Repositories;
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
        var existingUser = await _unitOfWork.GetRepository<UserRepository>().GetUsersByUsernameAndPasswordAsync(request.User.Username, request.User.Password);

        if (existingUser is null)
        {
            throw new InvalidCredentialsException(_localizationService);
        }

        var tokenService = new JwtService(_configuration);
        return new Tokens { Token = tokenService.GenerateToken(request.User) };
    }
}