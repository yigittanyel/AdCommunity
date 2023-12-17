using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.Ticket.Commands.UpdateTicketCommand;

public class UpdateTicketTypeCommandHandler : IYtRequestHandler<UpdateTicketTypeCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateTicketTypeCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(UpdateTicketTypeCommand request, CancellationToken cancellationToken)
    {
        var existingTicket = await _unitOfWork.GetRepository<TicketRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingTicket is null)
            throw new NotExistException("Ticket",_httpContextAccessor.HttpContext);

        var communityEvent = await _unitOfWork.GetRepository<EventRepository>().GetAsync(request.CommunityEventId, null, cancellationToken);

        if (communityEvent is null)
            throw new NotExistException("Event",_httpContextAccessor.HttpContext);

        existingTicket.AssignEvent(communityEvent);

        var community = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.CommunityId, null,cancellationToken);

        if (community is null)
            throw new NotExistException("Community",_httpContextAccessor.HttpContext);

        existingTicket.AssignCommunity(community);

        existingTicket.SetDate();

        _mapper.Map(request, existingTicket);

        _unitOfWork.GetRepository<TicketRepository>().Update(existingTicket);

        _rabbitMqFactory.PublishMessage("update_ticket_queue", $"Ticket with Id: {existingTicket.Id}  has been edited.");

        return true;
    }
}
