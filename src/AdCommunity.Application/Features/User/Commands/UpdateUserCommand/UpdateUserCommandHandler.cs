using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.Extensions.Localization;

namespace AdCommunity.Application.Features.User.Commands.UpdateUserCommand;

public class UpdateUserCommandHandler : IYtRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IStringLocalizerFactory _localizer;
    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IStringLocalizerFactory localizer)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _localizer = localizer;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUser == null)
        {
            throw new NotExistException((IStringLocalizer)_localizer, "User");         
        }

        existingUser.SetHashedPassword(request.Password);
        existingUser.SetDate();

        _mapper.Map(request, existingUser);

        _unitOfWork.GetRepository<UserRepository>().Update(existingUser);

        _rabbitMqFactory.PublishMessage("update_user_queue", $"User with Id: {existingUser.Id}  has been edited.");

        return true;
    }
}
