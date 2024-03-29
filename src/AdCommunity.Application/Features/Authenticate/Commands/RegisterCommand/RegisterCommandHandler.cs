﻿using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.Jwt;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.UnitOfWork;
using AdCommunity.Domain.Entities.SharedKernel;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.Authenticate.Commands.RegisterCommand;

public class RegisterCommandHandler : IYtRequestHandler<RegisterCommand, Tokens>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IJwtService _jwtService;
    public RegisterCommandHandler(IUnitOfWork unitOfWork, IJwtService jwtService)
    {
        _unitOfWork = unitOfWork;
        _jwtService = jwtService;
    }

    public async Task<Tokens> Handle(RegisterCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.GetRepository<UserRepository>().GetUsersByUsernameAndPasswordAsync(request.User.Username, request.User.Password, cancellationToken);

        if (existingUser is not null)
        {
            throw new AlreadyExistsException(existingUser.Username);
        }

        var user = new Domain.Entities.Aggregates.User.User(
            request.User.FirstName,
            request.User.LastName,
            request.User.Email,
            request.User.Password,
            request.User.Phone,
            request.User.Username,
            request.User.Website,
            request.User.Facebook,
            request.User.Twitter,
            request.User.Instagram,
            request.User.Github,
            request.User.Medium
        );

        await _unitOfWork.GetRepository<UserRepository>().AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var loginDto = new UserLoginDto(request.User.Username, request.User.Password);
        var token = _jwtService.GenerateToken(loginDto);
        return new Tokens { Token = token };
    }
}
