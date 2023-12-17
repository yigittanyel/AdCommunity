using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.Event.Commands.DeleteEventCommand;

public class DeleteEventCommandHandler : IYtRequestHandler<DeleteEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public DeleteEventCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.GetRepository<EventRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingEvent is null)
            throw new NotExistException("Event",_httpContextAccessor.HttpContext);

        var community = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(existingEvent.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException("Community",_httpContextAccessor.HttpContext);

        community.RemoveEvent(existingEvent);

        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("delete_event_queue", $"Event name: {existingEvent.EventName} has been removed.");
        return true;
    }
}