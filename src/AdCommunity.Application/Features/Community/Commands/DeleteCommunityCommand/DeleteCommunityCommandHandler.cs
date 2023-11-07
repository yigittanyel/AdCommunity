using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Community.Commands.DeleteCommunityCommand;

public class DeleteCommunityCommandHandler : IYtRequestHandler<DeleteCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;


    public DeleteCommunityCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory)
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

        _rabbitMqFactory.PublishMessage("delete_community_queue", $"Community name: {existingCommunity.Name} has been removed.");

        return true;
    }
}
