using AdCommunity.Application.Exceptions;
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

        if (existingUserTicket is null)
            throw new NotExistException("User Ticket");

        var user = await _unitOfWork.UserRepository.GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User");

        existingUserTicket.AssignUser(user);

        var ticket = await _unitOfWork.TicketRepository.GetAsync(request.TicketId, null, cancellationToken);

        if (ticket is null)
            throw new NotExistException("Ticket does not exist");

        existingUserTicket.AssignTicket(ticket);
        existingUserTicket.SetDate();

        _mapper.Map(request, existingUserTicket);

        _unitOfWork.UserTicketRepository.Update(existingUserTicket);

        _rabbitMqFactory.PublishMessage("update_userTicket_queue", $"User Ticket with Id: {existingUserTicket.Id}  has been edited.");

        return true;
    }
}
