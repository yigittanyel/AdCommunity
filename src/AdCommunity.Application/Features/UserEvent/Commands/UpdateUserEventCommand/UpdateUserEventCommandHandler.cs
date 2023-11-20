using AdCommunity.Application.Features.UserTicket.Commands.UpdateUserTicketCommand;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserEvent.Commands.UpdateUserEventCommand;

public class UpdateUserEventCommandHandler : IYtRequestHandler<UpdateUserEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public UpdateUserEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateUserEventCommand request, CancellationToken cancellationToken)
    {
        var existingUserEvent = await _unitOfWork.UserEventRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingUserEvent == null)
            throw new Exception("User Event does not exist");

        var user = await _unitOfWork.UserRepository.GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new Exception("User does not exist");

        var @event = await _unitOfWork.EventRepository.GetAsync(request.EventId, null, cancellationToken);
        if (@event is null)
            throw new Exception("Event does not exist");

        existingUserEvent.AssignUser(user);
        existingUserEvent.AssignEvent(@event);
        existingUserEvent.SetDate();

        _mapper.Map(request, existingUserEvent);

        _unitOfWork.UserEventRepository.Update(existingUserEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("update_userTicket_queue", $"User Ticket with Id: {existingUserEvent.Id}  has been edited.");

        return true;
    }
}