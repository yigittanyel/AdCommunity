using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.UserTicket.Commands.UpdateUserTicketCommand;
public class UpdateUserTicketCommandHandler : IYtRequestHandler<UpdateUserTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateUserTicketCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(UpdateUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.GetRepository<UserTicketRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUserTicket is null)
            throw new NotExistException("User Ticket",_httpContextAccessor.HttpContext);

        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User",_httpContextAccessor.HttpContext);

        existingUserTicket.AssignUser(user);

        var ticket = await _unitOfWork.GetRepository<TicketRepository>().GetAsync(request.TicketId, null, cancellationToken);

        if (ticket is null)
            throw new NotExistException("Ticket does not exist",_httpContextAccessor.HttpContext);

        existingUserTicket.AssignTicket(ticket);
        existingUserTicket.SetDate();

        _mapper.Map(request, existingUserTicket);

        _unitOfWork.GetRepository<UserTicketRepository>().Update(existingUserTicket);

        _rabbitMqFactory.PublishMessage("update_userTicket_queue", $"User Ticket with Id: {existingUserTicket.Id}  has been edited.");

        return true;
    }
}
