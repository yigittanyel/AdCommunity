using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Event.Commands.DeleteEventCommand;

public class DeleteEventCommandHandler : IYtRequestHandler<DeleteEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    public DeleteEventCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
    }
    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.EventRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingEvent is null)
            throw new NotExistException("Event");

        var community = await _unitOfWork.CommunityRepository.GetAsync(existingEvent.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException("Community");

        community.RemoveEvent(existingEvent);

        _unitOfWork.CommunityRepository.Update(community);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("delete_event_queue", $"Event name: {existingEvent.EventName} has been removed.");
        return true;
    }
}