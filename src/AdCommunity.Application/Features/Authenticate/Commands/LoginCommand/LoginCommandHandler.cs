using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Resources;
using AdCommunity.Application.Services.Jwt;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.SharedKernel;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace AdCommunity.Application.Features.Authenticate.Commands.LoginCommand;

public class LoginCommandHandler : IYtRequestHandler<LoginCommand, Tokens>
{
    private readonly IConfiguration _configuration;
    private readonly IUnitOfWork _unitOfWork;
    private readonly LocalizationService _localizationService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public LoginCommandHandler(IConfiguration configuration, IUnitOfWork unitOfWork, LocalizationService localizationService, IHttpContextAccessor httpContextAccessor)
    {
        _configuration = configuration;
        _unitOfWork = unitOfWork;
        _localizationService = localizationService;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Tokens> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.GetRepository<UserRepository>().GetUsersByUsernameAndPasswordAsync(request.User.Username, request.User.Password);

        if (existingUser is null)
        {
            var errorMessage = _localizationService.GetKey("InvalidCredentialsErrorMessage").Value;
            throw new InvalidCredentialsException(_httpContextAccessor.HttpContext);
        }

        var tokenService = new JwtService(_configuration);
        return new Tokens { Token = tokenService.GenerateToken(request.User) };
    }
}