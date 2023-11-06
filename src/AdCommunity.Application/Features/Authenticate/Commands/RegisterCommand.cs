﻿using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Services;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.Base;
using AdCommunity.Domain.Repository;
using Microsoft.Extensions.Configuration;

namespace AdCommunity.Application.Features.Authenticate.Commands;

public class RegisterCommand : IYtRequest<Tokens>
{
    public UserCreateDto User { get; set; }
}

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
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(request.User.Username, request.User.Password);

        if (existingUser != null && existingUser.Any())
        {
            throw new Exception("User already exists");
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

        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        var loginDto = new UserLoginDto(request.User.Username, request.User.Password);
        var token = _jwtService.GenerateToken(loginDto);
        return new Tokens { Token = token };
    }
}