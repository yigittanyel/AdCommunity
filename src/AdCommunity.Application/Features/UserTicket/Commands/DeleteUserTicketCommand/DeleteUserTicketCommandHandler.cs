using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;

using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.UserTicket.Commands.DeleteUserTicketCommand;
public class DeleteUserTicketCommandHandler : IYtRequestHandler<DeleteUserTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IStringLocalizerFactory _localizer;
    public DeleteUserTicketCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, IStringLocalizerFactory localizer)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _localizer = localizer;
    }

    public async Task<bool> Handle(DeleteUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.GetRepository<UserTicketRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUserTicket is null)
            throw new NotExistException((IStringLocalizer)_localizer, "User Ticket");

        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(existingUserTicket.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException((IStringLocalizer)_localizer, "User");

        user.RemoveUserTicket(existingUserTicket);

        _unitOfWork.GetRepository<UserRepository>().Update(user);

        _rabbitMqFactory.PublishMessage("delete_userTicket_queue", $"User Ticket with Id: {existingUserTicket.Id}  has been removed.");

        return true;
    }
}
