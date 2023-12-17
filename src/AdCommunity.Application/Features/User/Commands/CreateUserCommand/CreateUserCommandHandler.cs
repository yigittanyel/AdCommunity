using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.User.Commands.CreateUserCommand;

public class CreateUserCommandHandler : IYtRequestHandler<CreateUserCommand, UserCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IStringLocalizerFactory _localizer;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IStringLocalizerFactory localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizer = localizer;
    }

    public async Task<UserCreateDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.GetRepository<UserRepository>().GetUsersByUsernameAndPasswordAsync(request.Username, request.Password);

        if (existingUser is not null)
        {
            throw new AlreadyExistsException((IStringLocalizer)_localizer,"User");
        }

        var user = new Domain.Entities.Aggregates.User.User
        (request.FirstName, request.LastName, request.Email, request.Password, request.Phone, request.Username, request.Website, request.Facebook, request.Twitter, request.Instagram, request.Github, request.Medium);

        await _unitOfWork.GetRepository<UserRepository>().AddAsync(user, cancellationToken);

        _rabbitMqFactory.PublishMessage("create_user_queue", $"UserName: {user.Username} has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.User, UserCreateDto>(user);
    }
}