using AdCommunity.Application.DTOs.User;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.User.Commands;

public class UpdateUserCommand: IYtRequest<bool>
{
    public UserUpdateDto User { get; set; }
}

public class UpdateUserCommandHandler : IYtRequestHandler<UpdateUserCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly ConnectionFactory _rabbitMqFactory;

    public UpdateUserCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetAsync(request.User.Id,cancellationToken);

        if(existingUser == null)
        {
            throw new Exception("User does not exist");
        }

        existingUser.SetHashedPassword(request.User.Password);
        existingUser.SetDate();

        _mapper.Map(request.User, existingUser);

        _unitOfWork.UserRepository.Update(existingUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "update_user_queue", $"User with Id: {existingUser.Id}  has been edited.");

        return true;
    }
}
