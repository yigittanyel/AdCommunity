using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.UserTicket.Commands.CreateUserTicketCommand;
public class CreateUserTicketCommandHandler : IYtRequestHandler<CreateUserTicketCommand, UserTicketCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateUserTicketCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserTicketCreateDto> Handle(CreateUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.UserTicketRepository.GetUserTicketsByUserAndTicketAsync(request.UserId,request.TicketId,cancellationToken);

        if (existingUserTicket is not null)
            throw new AlreadyExistsException("User Ticket", _httpContextAccessor.HttpContext);

        var userTicket = Domain.Entities.Aggregates.User.UserTicket.Create(request.UserId,request.TicketId,request.Pnr);
          
        var user = await _unitOfWork.UserRepository.GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User",_httpContextAccessor.HttpContext);

        userTicket.AssignUser(user);

        var ticket = await _unitOfWork.TicketRepository.GetAsync(request.TicketId, null, cancellationToken);

        if (ticket is null)
            throw new NotExistException("Ticket",_httpContextAccessor.HttpContext);

        userTicket.AssignTicket(ticket);

        user.AddUserTicket(userTicket);

        _unitOfWork.UserRepository.Update(user);

        _rabbitMqFactory.PublishMessage("create_userTicket_queue", $"User Ticket has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.UserTicket, UserTicketCreateDto>(userTicket);
    }
}

