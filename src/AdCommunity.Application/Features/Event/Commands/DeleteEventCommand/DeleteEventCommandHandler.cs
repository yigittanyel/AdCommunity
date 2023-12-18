using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.Event.Commands.DeleteEventCommand;

public class DeleteEventCommandHandler : IYtRequestHandler<DeleteEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService ;
    public DeleteEventCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }
    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.GetRepository<EventRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingEvent is null)
            throw new NotExistException(_localizationService, "Event");

        var community = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(existingEvent.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException(_localizationService, "Community");

        community.RemoveEvent(existingEvent);

        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("delete_event_queue", $"Event name: {existingEvent.EventName} has been removed.");
        return true;
    }
}