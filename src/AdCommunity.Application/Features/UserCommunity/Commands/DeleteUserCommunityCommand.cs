using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.UserCommunity.Commands;

public class DeleteUserCommunityCommand:IYtRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteUserCommunityCommandHandler : IYtRequestHandler<DeleteUserCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConnectionFactory _rabbitMqFactory;


    public DeleteUserCommunityCommandHandler(IUnitOfWork unitOfWork, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(DeleteUserCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingUserCommunity = await _unitOfWork.UserCommunityRepository.GetAsync(request.Id, cancellationToken);

        if (existingUserCommunity == null)
        {
            throw new Exception("UserCommunity does not exist");
        }

        _unitOfWork.UserCommunityRepository.Delete(existingUserCommunity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "delete_userCommunity_queue", $"The UserCommunity with ID: {request.Id} has been removed.");
        return true;
    }
}