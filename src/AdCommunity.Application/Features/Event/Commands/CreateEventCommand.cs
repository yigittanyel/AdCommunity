using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Helpers;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Event.Commands;

public class CreateEventCommand:IYtRequest<EventCreateDto>
{
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
    public int CommunityId { get; set; }

    public CreateEventCommand(string eventName, string description, DateTime eventDate, string location, int communityId)
    {
        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CommunityId = communityId;
    }
}

public class CreateEventCommandHandler : IYtRequestHandler<CreateEventCommand, EventCreateDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly ConnectionFactory _rabbitMqFactory;

    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
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

        var _event = new AdCommunity.Domain.Entities.Aggregates.Community.Event(request.EventName, request.Description, request.EventDate, request.Location, request.CommunityId);



        await _unitOfWork.EventRepository.AddAsync(_event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "create_event_queue", $"Event name: {_event.EventName} has been created.");

        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.Community.Event, EventCreateDto>(_event);
    }
}
