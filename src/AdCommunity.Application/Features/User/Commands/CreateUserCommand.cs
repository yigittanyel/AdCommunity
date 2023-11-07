using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Helpers;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using FluentValidation;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.User.Commands;

public class CreateUserCommand : IYtRequest<UserCreateDto>
{
    public string FirstName { get; set; }
    public string Password { get; set; }
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

    public CreateUserCommand(string firstName, string password, string lastName, string email, string phone, string username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        FirstName = firstName;
        Password = password;
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
    }
}

public class CreateUserCommandHandler : IYtRequestHandler<CreateUserCommand, UserCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly ConnectionFactory _rabbitMqFactory;

    public CreateUserCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
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

        var user = new AdCommunity.Domain.Entities.Aggregates.User.User
        (request.FirstName, request.LastName, request.Email, request.Password, request.Phone, request.Username, request.Website, request.Facebook, request.Twitter, request.Instagram, request.Github, request.Medium);

        await _unitOfWork.UserRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "create_user_queue", $"UserName: {user.Username} has been created.");

        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.User.User, UserCreateDto>(user);
    }
}