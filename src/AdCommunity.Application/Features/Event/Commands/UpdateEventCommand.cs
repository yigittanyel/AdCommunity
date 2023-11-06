using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.DTOs.Event;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using FluentValidation;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Event.Commands;

public class UpdateEventCommand:IYtRequest<bool>
{
    public EventUpdateDto Event { get; set; }
}

public class UpdateEventCommandHandler : IYtRequestHandler<UpdateEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly ConnectionFactory _rabbitMqFactory;

    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }
    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.EventRepository.GetAsync(request.Event.Id, cancellationToken);

        if (existingEvent == null)
        {
            throw new Exception("Event does not exist");
        }

        existingEvent.SetDate();

        _mapper.Map(request.Event, existingEvent);

        var validationResult = await new EventUpdateDtoValidator().ValidateAsync(request.Event);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }

        _unitOfWork.EventRepository.Update(existingEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "update_event_queue", $"Event with Id: {existingEvent.Id}  has been edited.");

        return true;
    }
}
