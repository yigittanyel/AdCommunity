using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Community.Commands;

public class DeleteCommunityCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteCommunityCommandHandler : IYtRequestHandler<DeleteCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConnectionFactory _rabbitMqFactory;


    public DeleteCommunityCommandHandler(IUnitOfWork unitOfWork, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.CommunityRepository.GetAsync(request.Id, cancellationToken);

        if (existingCommunity == null)
        {
            throw new Exception("Community does not exist");
        }

        _unitOfWork.CommunityRepository.Delete(existingCommunity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "delete_community_queue", $"Community name: {existingCommunity.Name} has been removed.");

        return true;
    }
}
