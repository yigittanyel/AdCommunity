using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;
using System.Text;

namespace AdCommunity.Application.Features.User.Commands;

public class DeleteUserCommand : IYtRequest<bool>
{
    public int Id { get; set; }
}

public class DeleteUserCommandHandler : IYtRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ConnectionFactory _rabbitMqFactory;


    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetAsync(request.Id, cancellationToken);

        if (existingUser == null)
        {
            throw new Exception("User does not exist");
        }

        _unitOfWork.UserRepository.Delete(existingUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "delete_user_queue", "User has been removed.");

        return true;
    }
}
