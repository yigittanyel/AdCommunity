using AdCommunity.Application.DTOs.Event;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;

using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.Event.Commands.CreateEventCommand;

public class CreateEventCommandHandler : IYtRequestHandler<CreateEventCommand, EventCreateDto>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IStringLocalizerFactory _localizer;


    public CreateEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IStringLocalizerFactory localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizer = localizer;
    }


    public async Task<EventCreateDto> Handle(CreateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.GetRepository<EventRepository>().GetByEventNameAsync(request.EventName, cancellationToken);

        if (existingEvent is not null)
            throw new AlreadyExistsException((IStringLocalizer)_localizer,existingEvent.EventName);

        var community= await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.CommunityId, null, cancellationToken);

        if (community is null) 
            throw new NotExistException((IStringLocalizer)_localizer,"Community");

        var @event = new Domain.Entities.Aggregates.Community.Event(request.EventName, request.Description, request.EventDate, request.Location);

        @event.AssignCommunity(community);

        community.AddEvent(@event);
     
        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("create_event_queue", $"Event name: {@event.EventName} has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.Community.Event, EventCreateDto>(@event);
    }
}
