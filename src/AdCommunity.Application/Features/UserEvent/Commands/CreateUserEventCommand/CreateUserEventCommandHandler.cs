using AdCommunity.Application.DTOs.UserEvent;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.UserEvent.Commands.CreateUserEventCommand;

public class CreateUserEventCommandHandler : IYtRequestHandler<CreateUserEventCommand, UserEventCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateUserEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserEventCreateDto> Handle(CreateUserEventCommand request, CancellationToken cancellationToken)
    {
        var existingUserEvent = await _unitOfWork.GetRepository<UserEventRepository>().GetUserEventsByUserAndEventAsync(request.UserId, request.EventId, cancellationToken);

        if (existingUserEvent is not null)
            throw new AlreadyExistsException("User Event", _httpContextAccessor.HttpContext);

        var userEvent = Domain.Entities.Aggregates.User.UserEvent.Create(request.UserId, request.EventId);

        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User",_httpContextAccessor.HttpContext);

        userEvent.AssignUser(user);

        var @event = await _unitOfWork.GetRepository<EventRepository>().GetAsync(request.EventId, null, cancellationToken);

        if (@event is null)
            throw new NotExistException("Event",_httpContextAccessor.HttpContext);

        userEvent.AssignEvent(@event);

        user.AddUserEvent(userEvent);

        _unitOfWork.GetRepository<UserRepository>().Update(user);

        _rabbitMqFactory.PublishMessage("create_userEvent_queue", $"User Event has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.UserEvent, UserEventCreateDto>(userEvent);
    }
}