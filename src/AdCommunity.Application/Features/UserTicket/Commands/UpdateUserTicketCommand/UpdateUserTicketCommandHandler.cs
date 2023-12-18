using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.UserTicket.Commands.UpdateUserTicketCommand;
public class UpdateUserTicketCommandHandler : IYtRequestHandler<UpdateUserTicketCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService;
    public UpdateUserTicketCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }

    public async Task<bool> Handle(UpdateUserTicketCommand request, CancellationToken cancellationToken)
    {
        var existingUserTicket = await _unitOfWork.GetRepository<UserTicketRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUserTicket is null)
            throw new NotExistException(_localizationService, "User Ticket");

        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException(_localizationService, "User");

        existingUserTicket.AssignUser(user);

        var ticket = await _unitOfWork.GetRepository<TicketRepository>().GetAsync(request.TicketId, null, cancellationToken);

        if (ticket is null)
            throw new NotExistException(_localizationService, "Ticket does not exist");

        existingUserTicket.AssignTicket(ticket);
        existingUserTicket.SetDate();

        _mapper.Map(request, existingUserTicket);

        _unitOfWork.GetRepository<UserTicketRepository>().Update(existingUserTicket);

        _rabbitMqFactory.PublishMessage("update_userTicket_queue", $"User Ticket with Id: {existingUserTicket.Id}  has been edited.");

        return true;
    }
}
