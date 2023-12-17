using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;

using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.UserEvent.Commands.DeleteUserEventCommand;

public class DeleteUserEventCommandHandler : IYtRequestHandler<DeleteUserEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IStringLocalizerFactory _localizer;
    public DeleteUserEventCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, IStringLocalizerFactory localizer)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _localizer = localizer;
    }

    public async Task<bool> Handle(DeleteUserEventCommand request, CancellationToken cancellationToken)
    {
        var existingUserEvent = await _unitOfWork.GetRepository<UserEventRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUserEvent is null)
            throw new NotExistException((IStringLocalizer)_localizer, "User Event");

        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(existingUserEvent.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException((IStringLocalizer)_localizer, "User");

        user.RemoveUserEvent(existingUserEvent);

        _unitOfWork.GetRepository<UserRepository>().Update(user);

        _rabbitMqFactory.PublishMessage("delete_userEvent_queue", $"User Ticket with Id: {existingUserEvent.Id}  has been removed.");

        return true;
    }
}
