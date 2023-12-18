using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.Ticket.Commands.DeleteTicketCommand;

public class DeleteTicketTypeCommandHandler : IYtRequestHandler<DeleteTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService;

    public DeleteTicketTypeCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }

    public async Task<bool> Handle(DeleteTicketCommand request, CancellationToken cancellationToken)
    {
        var existingTicket = await _unitOfWork.GetRepository<TicketRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingTicket is null)
            throw new NotExistException(_localizationService, "Ticket");


        var community = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(existingTicket.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException(_localizationService, "Community");

        community.RemoveTicket(existingTicket);

        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("delete_ticket_queue", $"Ticket has been removed.");

        return true;
    }
}

