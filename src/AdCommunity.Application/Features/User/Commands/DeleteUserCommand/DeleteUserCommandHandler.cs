using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.User.Commands.DeleteUserCommand;

public class DeleteUserCommandHandler : IYtRequestHandler<DeleteUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    public DeleteUserCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(DeleteUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUser == null)
        {
            throw new NotExistException("User");
        }

        _unitOfWork.GetRepository<UserRepository>().Delete(existingUser);

        _rabbitMqFactory.PublishMessage("delete_user_queue", $"User with Id: {existingUser.Id}  has been removed.");

        return true;
    }
}
