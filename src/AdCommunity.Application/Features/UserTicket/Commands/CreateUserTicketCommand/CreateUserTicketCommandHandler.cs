using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;

using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.UserTicket.Commands.CreateUserTicketCommand;
public class CreateUserTicketCommandHandler : IYtRequestHandler<CreateUserTicketCommand, UserTicketCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IStringLocalizerFactory _localizer;
    public CreateUserTicketCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IStringLocalizerFactory localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizer = localizer;
    }

    public async Task<UserTicketCreateDto> Handle(CreateUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.GetRepository<UserTicketRepository>().GetUserTicketsByUserAndTicketAsync(request.UserId,request.TicketId,cancellationToken);

        if (existingUserTicket is not null)
            throw new AlreadyExistsException((IStringLocalizer)_localizer, "User Ticket");

        var userTicket = Domain.Entities.Aggregates.User.UserTicket.Create(request.UserId,request.TicketId,request.Pnr);
          
        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException((IStringLocalizer)_localizer, "User");

        userTicket.AssignUser(user);

        var ticket = await _unitOfWork.GetRepository<TicketRepository>().GetAsync(request.TicketId, null, cancellationToken);

        if (ticket is null)
            throw new NotExistException((IStringLocalizer)_localizer, "Ticket");

        userTicket.AssignTicket(ticket);

        user.AddUserTicket(userTicket);

        _unitOfWork.GetRepository<UserRepository>().Update(user);

        _rabbitMqFactory.PublishMessage("create_userTicket_queue", $"User Ticket has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.UserTicket, UserTicketCreateDto>(userTicket);
    }
}

