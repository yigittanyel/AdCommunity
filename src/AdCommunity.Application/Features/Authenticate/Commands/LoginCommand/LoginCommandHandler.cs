using AdCommunity.Application.Services.Jwt;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.SharedKernel;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.Authenticate.Commands.LoginCommand;

public class LoginCommandHandler : IYtRequestHandler<LoginCommand, Tokens>
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IStringLocalizerFactory _localizer;

    public LoginCommandHandler(IConfiguration configuration, IUnitOfWork unitOfWork, IStringLocalizerFactory localizer)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _localizer = localizer;
    }

    public async Task<Tokens> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.GetRepository<UserRepository>().GetUsersByUsernameAndPasswordAsync(request.User.Username, request.User.Password);

        if (existingUser is null)
        {
            throw new InvalidCredentialsException((IStringLocalizer)_localizer);
        }

        var tokenService = new JwtService(_configuration);
        return new Tokens { Token = tokenService.GenerateToken(request.User) };
    }
}