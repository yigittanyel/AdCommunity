using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.Event.Commands.UpdateEventCommand;
public class UpdateEventCommandHandler : IYtRequestHandler<UpdateEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }
    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.EventRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingEvent is null)
            throw new NotExistException("Event",_httpContextAccessor.HttpContext);

        var community = await _unitOfWork.CommunityRepository.GetAsync(request.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException("Community",_httpContextAccessor.HttpContext);

        existingEvent.SetDate();
        _mapper.Map(request, existingEvent);

        _unitOfWork.EventRepository.Update(existingEvent);

        _rabbitMqFactory.PublishMessage("update_event_queue", $"Event with Id: {existingEvent.Id}  has been edited.");

        return true;
    }
}
