using AdCommunity.Application.DTOs.UserTicket;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserTicket.Commands.CreateUserTicketCommand;

public class CreateUserTicketCommandHandler : IYtRequestHandler<CreateUserTicketCommand, UserTicketCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public CreateUserTicketCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<UserTicketCreateDto> Handle(CreateUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.UserTicketRepository.GetUserTicketsByUserAndTicketAsync(request.UserId,request.TicketId,cancellationToken);

        if (existingUserTicket is not null)
            throw new Exception("User Ticket already exists");

        var userTicket = Domain.Entities.Aggregates.User.UserTicket.Create(request.UserId,request.TicketId,request.Pnr);

        var user = await _unitOfWork.UserRepository.GetAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new Exception("User does not exist");

        var ticket = await _unitOfWork.TicketRepository.GetAsync(request.TicketId, cancellationToken);
        if (ticket is null)
            throw new Exception("Ticket does not exist");

        await _unitOfWork.UserTicketRepository.AddAsync(userTicket, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_userTicket_queue", $"User Ticket has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.UserTicket, UserTicketCreateDto>(userTicket);
    }
}

