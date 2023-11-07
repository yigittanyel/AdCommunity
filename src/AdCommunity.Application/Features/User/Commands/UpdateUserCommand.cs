using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.User.Commands;

public class UpdateUserCommand: IYtRequest<bool>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Username { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Instagram { get; set; }
    public string? Github { get; set; }
    public string? Medium { get; set; }
    public string Password { get; set; }

    public UpdateUserCommand(int id, string firstName, string lastName, string email, string phone, string username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Username = username;
        Website = website;
        Facebook = facebook;
        Twitter = twitter;
        Instagram = instagram;
        Github = github;
        Medium = medium;
        Password = password;
    }
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
        var existingUser = await _unitOfWork.UserRepository.GetAsync(request.Id,cancellationToken);

        if(existingUser == null)
        {
            throw new Exception("User does not exist");
        }

        existingUser.SetHashedPassword(request.Password);
        existingUser.SetDate();

        _mapper.Map(request, existingUser);

        _unitOfWork.UserRepository.Update(existingUser);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "update_user_queue", $"User with Id: {existingUser.Id}  has been edited.");

        return true;
    }
}
