using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Ticket.Commands.DeleteTicketCommand;

public class DeleteTicketTypeCommandHandler : IYtRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;


    public DeleteTicketTypeCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var existingTicket = await _unitOfWork.TicketRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingTicket is null)
            throw new NotExistException("Ticket");


        var community = await _unitOfWork.CommunityRepository.GetAsync(existingTicket.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException("Community");

        community.RemoveTicket(existingTicket);

        _unitOfWork.TicketRepository.Delete(existingTicket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("delete_ticket_queue", $"Ticket has been removed.");

        return true;
    }
}

