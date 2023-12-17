using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.UserEvent.Commands.DeleteUserEventCommand;

public class DeleteUserEventCommandHandler : IYtRequestHandler<DeleteUserEventCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteUserEventCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteUserEventCommand request, CancellationToken cancellationToken)
    {
        var existingUserEvent = await _unitOfWork.GetRepository<UserEventRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUserEvent is null)
            throw new NotExistException("User Event",_httpContextAccessor.HttpContext);

        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(existingUserEvent.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User",_httpContextAccessor.HttpContext);

        user.RemoveUserEvent(existingUserEvent);

        _unitOfWork.GetRepository<UserRepository>().Update(user);

        _rabbitMqFactory.PublishMessage("delete_userEvent_queue", $"User Ticket with Id: {existingUserEvent.Id}  has been removed.");

        return true;
    }
}
