using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Event.Commands;

public class DeleteEventCommand:IYtRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteEventCommandHandler : IYtRequestHandler<DeleteEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConnectionFactory _rabbitMqFactory;
    public DeleteEventCommandHandler(IUnitOfWork unitOfWork, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
    }
    public async Task<bool> Handle(DeleteEventCommand request, CancellationToken cancellationToken)
    {
        var existingEvent = await _unitOfWork.EventRepository.GetAsync(request.Id, cancellationToken);

        if (existingEvent == null)
        {
            throw new Exception("Event does not exist");
        }

        _unitOfWork.EventRepository.Delete(existingEvent);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "delete_event_queue", $"Event name: {existingEvent.EventName} has been removed.");

        return true;
    }
}