using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Helpers;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using FluentValidation;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Event.Commands;

public class CreateEventCommand:IYtRequest<EventCreateDto>
{
    public EventCreateDto Event { get; set; }
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
        var existingEvent = await _unitOfWork.EventRepository.GetByEventNameAsync(request.Event.EventName, cancellationToken);

        if (existingEvent is not null)
        {
            throw new Exception("Event already exists");
        }

        var _event = new AdCommunity.Domain.Entities.Aggregates.Community.Event(request.Event.EventName, request.Event.Description, request.Event.EventDate, request.Event.Location, request.Event.CommunityId);

        var validationResult = await new EventCreateDtoValidator().ValidateAsync(request.Event);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }


        await _unitOfWork.EventRepository.AddAsync(_event, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "create_event_queue", $"Event name: {_event.EventName} has been created.");

        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.Community.Event, EventCreateDto>(_event);
    }
}
