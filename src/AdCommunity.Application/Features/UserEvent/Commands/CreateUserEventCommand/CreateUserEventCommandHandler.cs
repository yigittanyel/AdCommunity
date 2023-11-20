using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserEvent.Commands.CreateUserEventCommand;

public class CreateUserEventCommandHandler : IYtRequestHandler<CreateUserEventCommand, UserEventCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public CreateUserEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<UserEventCreateDto> Handle(CreateUserEventCommand request, CancellationToken cancellationToken)
    {
        var existingUserEvent = await _unitOfWork.UserEventRepository.GetUserEventsByUserAndEventAsync(request.UserId, request.EventId, cancellationToken);

        if (existingUserEvent is not null)
            throw new Exception("User Event already exists");

        var userEvent = Domain.Entities.Aggregates.User.UserEvent.Create(request.UserId, request.EventId);

        if (userEvent is null)
            throw new Exception("User Event does not exist");

        var user = await _unitOfWork.UserRepository.GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new Exception("User does not exist");

        userEvent.AssignUser(user);

        var @event = await _unitOfWork.EventRepository.GetAsync(request.EventId, null, cancellationToken);
        if (@event is null)
            throw new Exception("Event does not exist");
        userEvent.AssignEvent(@event);

        user.AddUserEvent(userEvent);

        await _unitOfWork.UserEventRepository.AddAsync(userEvent, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_userEvent_queue", $"User Event has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.UserEvent, UserEventCreateDto>(userEvent);
    }
}