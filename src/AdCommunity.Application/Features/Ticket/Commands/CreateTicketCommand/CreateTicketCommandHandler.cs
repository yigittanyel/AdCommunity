using AdCommunity.Application.DTOs.Ticket;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Ticket.Commands.CreateTicketCommand;

public class CreateTicketCommandHandler : IYtRequestHandler<CreateTicketCommand, TicketCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public CreateTicketCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }
    public async Task<TicketCreateDto> Handle(CreateTicketCommand request, CancellationToken cancellationToken)
    {
        var existingTicket = await _unitOfWork.TicketRepository.GetTicketByEventAndCommunityIdsAsync(request.CommunityEventId, request.CommunityId, cancellationToken);

        if (existingTicket is not null)
            throw new Exception("Ticket already exists");

        var ticket = Domain.Entities.Aggregates.Community.Ticket.Create(request.CommunityEventId, request.CommunityId, request.Price);

        var communityEvent = await _unitOfWork.EventRepository.GetAsync(request.CommunityEventId, cancellationToken);

        if (communityEvent is null)
            throw new Exception("Event does not exist");

        var community = await _unitOfWork.CommunityRepository.GetAsync(request.CommunityId, cancellationToken);

        if (community is null)
            throw new Exception("Community does not exist");

        await _unitOfWork.TicketRepository.AddAsync(ticket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_ticket_queue", $"Ticket has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.Community.Ticket, TicketCreateDto>(ticket);
    }
}
