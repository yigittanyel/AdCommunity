using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Event.Commands;

public class UpdateEventCommand:IYtRequest<bool>
{
    public int Id { get; set; }
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
    public int CommunityId { get; set; }

    public UpdateEventCommand(int id, string eventName, string description, DateTime eventDate, string location, int communityId)
    {
        Id = id;
        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
        CommunityId = communityId;
    }
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
        var existingEvent = await _unitOfWork.EventRepository.GetAsync(request.Id, cancellationToken);

        if (existingEvent == null)
        {
            throw new Exception("Event does not exist");
        }

        existingEvent.SetDate();

        _mapper.Map(request, existingEvent);


        _unitOfWork.EventRepository.Update(existingEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "update_event_queue", $"Event with Id: {existingEvent.Id}  has been edited.");

        return true;
    }
}
