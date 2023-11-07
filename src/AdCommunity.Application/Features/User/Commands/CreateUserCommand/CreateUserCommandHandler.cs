using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.User.Commands.CreateUserCommand;

public class CreateUserCommandHandler : IYtRequestHandler<CreateUserCommand, UserCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<UserCreateDto> Handle(CreateUserCommand request, CancellationToken cancellationToken)
    {
        var existingUser = await _unitOfWork.UserRepository.GetUsersByUsernameAndPasswordAsync(request.Username, request.Password);

        if (existingUser.Any())
        {
            throw new Exception("User already exists");
        }

        var user = new Domain.Entities.Aggregates.User.User
        (request.FirstName, request.LastName, request.Email, request.Password, request.Phone, request.Username, request.Website, request.Facebook, request.Twitter, request.Instagram, request.Github, request.Medium);

        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_user_queue", $"UserName: {user.Username} has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.User, UserCreateDto>(user);
    }
}