using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.Ticket.Commands.DeleteTicketCommand;

public class DeleteTicketTypeCommandHandler : IYtRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteTicketTypeCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var existingTicket = await _unitOfWork.GetRepository<TicketRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingTicket is null)
            throw new NotExistException("Ticket",_httpContextAccessor.HttpContext);


        var community = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(existingTicket.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException("Community",_httpContextAccessor.HttpContext);

        community.RemoveTicket(existingTicket);

        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("delete_ticket_queue", $"Ticket has been removed.");

        return true;
    }
}

