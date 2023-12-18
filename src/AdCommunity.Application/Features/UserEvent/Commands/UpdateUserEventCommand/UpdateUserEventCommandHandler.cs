using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.UserEvent.Commands.UpdateUserEventCommand;

public class UpdateUserEventCommandHandler : IYtRequestHandler<UpdateUserEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService;
    public UpdateUserEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }

    public async Task<bool> Handle(UpdateUserEventCommand request, CancellationToken cancellationToken)
    {
        var existingUserEvent = await _unitOfWork.GetRepository<UserEventRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUserEvent is null)
            throw new NotExistException(_localizationService, "User Ticket");

        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException(_localizationService, "User");

        existingUserEvent.AssignUser(user);

        var @event = await _unitOfWork.GetRepository<EventRepository>().GetAsync(request.EventId, null, cancellationToken);

        if (@event is null)
            throw new NotExistException(_localizationService, "Event");

        existingUserEvent.AssignEvent(@event);
        existingUserEvent.SetDate();

        _mapper.Map(request, existingUserEvent);

        _unitOfWork.GetRepository<UserEventRepository>().Update(existingUserEvent);

        _rabbitMqFactory.PublishMessage("update_userTicket_queue", $"User Ticket with Id: {existingUserEvent.Id}  has been edited.");

        return true;
    }
}