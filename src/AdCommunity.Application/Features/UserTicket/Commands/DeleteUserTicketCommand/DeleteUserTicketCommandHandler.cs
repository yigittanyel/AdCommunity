using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.UserTicket.Commands.DeleteUserTicketCommand;
public class DeleteUserTicketCommandHandler : IYtRequestHandler<DeleteUserTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteUserTicketCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.UserTicketRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingUserTicket is null)
            throw new NotExistException("User Ticket",_httpContextAccessor.HttpContext);

        var user = await _unitOfWork.UserRepository.GetAsync(existingUserTicket.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User",_httpContextAccessor.HttpContext);

        user.RemoveUserTicket(existingUserTicket);

        _unitOfWork.UserRepository.Update(user);

        _rabbitMqFactory.PublishMessage("delete_userTicket_queue", $"User Ticket with Id: {existingUserTicket.Id}  has been removed.");

        return true;
    }
}
