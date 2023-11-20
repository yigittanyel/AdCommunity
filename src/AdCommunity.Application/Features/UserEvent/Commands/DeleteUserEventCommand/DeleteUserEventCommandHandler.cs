using AdCommunity.Application.Features.UserTicket.Commands.DeleteUserTicketCommand;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserEvent.Commands.DeleteUserEventCommand;

public class DeleteUserEventCommandHandler : IYtRequestHandler<DeleteUserEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;


    public DeleteUserEventCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(DeleteUserEventCommand request, CancellationToken cancellationToken)
    {
        var existingUserEvent = await _unitOfWork.UserEventRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingUserEvent == null)
        {
            throw new Exception("User Event does not exist");
        }

        _unitOfWork.UserEventRepository.Delete(existingUserEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("delete_userEvent_queue", $"User Ticket with Id: {existingUserEvent.Id}  has been removed.");

        return true;
    }
}
