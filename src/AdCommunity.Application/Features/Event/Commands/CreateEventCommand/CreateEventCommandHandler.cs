using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.Event.Commands.CreateEventCommand;

public class CreateEventCommandHandler : IYtRequestHandler<CreateEventCommand, EventCreateDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<EventCreateDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.GetRepository<EventRepository>().GetByEventNameAsync(request.EventName, cancellationToken);

        if (existingEvent is not null)
            throw new AlreadyExistsException(existingEvent.EventName);

        var community= await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.CommunityId, null, cancellationToken);

        if (community is null) 
            throw new NotExistException("Community");

        var @event = new Domain.Entities.Aggregates.Community.Event(request.EventName, request.Description, request.EventDate, request.Location);

        @event.AssignCommunity(community);

        community.AddEvent(@event);
     
        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("create_event_queue", $"Event name: {@event.EventName} has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.Community.Event, EventCreateDto>(@event);
    }
}
