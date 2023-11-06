﻿using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Helpers;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using FluentValidation;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.User.Commands;

public class CreateUserCommand : IYtRequest<UserCreateDto>
{
    public UserCreateDto User { get; set; }
}

public class CreateUserCommandHandler : IYtRequestHandler<CreateUserCommand, UserCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly ConnectionFactory _rabbitMqFactory;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<UserCreateDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(request.User.Username, request.User.Password);

        if (existingUser.Any())
        {
            throw new Exception("User already exists");
        }

        var user = new AdCommunity.Domain.Entities.Aggregates.User.User
        (request.User.FirstName, request.User.LastName, request.User.Email, request.User.Password, request.User.Phone, request.User.Username, request.User.Website, request.User.Facebook, request.User.Twitter, request.User.Instagram, request.User.Github, request.User.Medium);

        var validationResult = await new UserCreateDtoValidator().ValidateAsync(request.User);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }


        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "create_user_queue", $"UserName: {user.Username} has been created.");

        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.User.User, UserCreateDto>(user);
    }
}