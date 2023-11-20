using AdCommunity.Application.Features.Community.Commands.UpdateCommunityCommand;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserTicket.Commands.UpdateUserTicketCommand;


public class UpdateUserTicketCommandHandler : IYtRequestHandler<UpdateUserTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public UpdateUserTicketCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.UserTicketRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingUserTicket == null)
            throw new Exception("User Ticket does not exist");

        var user = await _unitOfWork.UserRepository.GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new Exception("User does not exist");

        var ticket = await _unitOfWork.TicketRepository.GetAsync(request.TicketId, null, cancellationToken);
        if (ticket is null)
            throw new Exception("Ticket does not exist");

        existingUserTicket.AssignUser(user);
        existingUserTicket.AssignTicket(ticket);
        existingUserTicket.SetDate();

        _mapper.Map(request, existingUserTicket);

        _unitOfWork.UserTicketRepository.Update(existingUserTicket);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("update_userTicket_queue", $"User Ticket with Id: {existingUserTicket.Id}  has been edited.");

        return true;
    }
}
