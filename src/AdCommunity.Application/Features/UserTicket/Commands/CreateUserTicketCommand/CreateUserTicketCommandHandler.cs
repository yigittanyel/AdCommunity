using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.UserTicket.Commands.CreateUserTicketCommand;
public class CreateUserTicketCommandHandler : IYtRequestHandler<CreateUserTicketCommand, UserTicketCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService;
    public CreateUserTicketCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }

    public async Task<UserTicketCreateDto> Handle(CreateUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.GetRepository<UserTicketRepository>().GetUserTicketsByUserAndTicketAsync(request.UserId,request.TicketId,cancellationToken);

        if (existingUserTicket is not null)
            throw new AlreadyExistsException(_localizationService, "User Ticket");

        var userTicket = Domain.Entities.Aggregates.User.UserTicket.Create(request.UserId,request.TicketId,request.Pnr);
          
        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException(_localizationService, "User");

        userTicket.AssignUser(user);

        var ticket = await _unitOfWork.GetRepository<TicketRepository>().GetAsync(request.TicketId, null, cancellationToken);

        if (ticket is null)
            throw new NotExistException(_localizationService, "Ticket");

        userTicket.AssignTicket(ticket);

        user.AddUserTicket(userTicket);

        _unitOfWork.GetRepository<UserRepository>().Update(user);

        _rabbitMqFactory.PublishMessage("create_userTicket_queue", $"User Ticket has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.UserTicket, UserTicketCreateDto>(userTicket);
    }
}

