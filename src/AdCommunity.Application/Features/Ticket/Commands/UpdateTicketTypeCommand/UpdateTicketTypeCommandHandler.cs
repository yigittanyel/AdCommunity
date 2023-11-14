using AdCommunity.Application.Features.Community.Commands.UpdateCommunityCommand;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Ticket.Commands.UpdateTicketCommand;

public class UpdateTicketTypeCommandHandler : IYtRequestHandler<UpdateTicketTypeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public UpdateTicketTypeCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var existingTicket = await _unitOfWork.TicketRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingTicket == null)
            throw new Exception("Ticket does not exist");

        var communityEvent = await _unitOfWork.EventRepository.GetAsync(request.CommunityEventId, null, cancellationToken);

        if (communityEvent is null)
            throw new Exception("Event does not exist");

        var community = await _unitOfWork.CommunityRepository.GetAsync(request.CommunityId, null,cancellationToken);

        if (community is null)
            throw new Exception("Community does not exist");

        existingTicket.SetDate();

        _mapper.Map(request, existingTicket);

        _unitOfWork.TicketRepository.Update(existingTicket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("update_ticket_queue", $"Ticket with Id: {existingTicket.Id}  has been edited.");

        return true;
    }
}
