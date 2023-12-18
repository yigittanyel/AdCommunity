using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.Event.Commands.UpdateEventCommand;
public class UpdateEventCommandHandler : IYtRequestHandler<UpdateEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService;
    public UpdateEventCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }
    public async Task<bool> Handle(UpdateEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.GetRepository<EventRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingEvent is null)
            throw new NotExistException(_localizationService, "Event");

        var community = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException(_localizationService, "Community");

        existingEvent.SetDate();
        _mapper.Map(request, existingEvent);

        _unitOfWork.GetRepository<EventRepository>().Update(existingEvent);

        _rabbitMqFactory.PublishMessage("update_event_queue", $"Event with Id: {existingEvent.Id}  has been edited.");

        return true;
    }
}
