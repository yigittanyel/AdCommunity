using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

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
        var existingEvent = await _unitOfWork.EventRepository.GetByEventNameAsync(request.EventName, cancellationToken);

        if (existingEvent is not null)
        {
            throw new Exception("Event already exists");
        }

        var community= await _unitOfWork.CommunityRepository.GetAsync(request.CommunityId, null, cancellationToken);

        if (community is null) 
            throw new Exception("Community does not exist");

        var @event = new Domain.Entities.Aggregates.Community.Event(request.EventName, request.Description, request.EventDate, request.Location);

        if (@event is null)
            throw new Exception("Event does not exist");

        @event.AssignCommunity(community);

        community.AddEvent(@event);
            
        await _unitOfWork.EventRepository.AddAsync(@event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_event_queue", $"Event name: {@event.EventName} has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.Community.Event, EventCreateDto>(@event);
    }
}
