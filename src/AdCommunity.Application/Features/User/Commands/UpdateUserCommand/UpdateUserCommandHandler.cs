﻿using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.User.Commands.UpdateUserCommand;

public class UpdateUserCommandHandler : IYtRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUser == null)
        {
            throw new NotExistException("User");         
        }

        existingUser.SetHashedPassword(request.Password);
        existingUser.SetDate();

        _mapper.Map(request, existingUser);

        _unitOfWork.GetRepository<UserRepository>().Update(existingUser);

        _rabbitMqFactory.PublishMessage("update_user_queue", $"User with Id: {existingUser.Id}  has been edited.");

        return true;
    }
}
