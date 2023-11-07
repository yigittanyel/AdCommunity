using AdCommunity.Application.DTOs.Community;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using FluentValidation;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Community.Commands;

public class UpdateCommunityCommand:IYtRequest<bool>
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }
    public string? Location { get; set; }
    public string? Organizators { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Instagram { get; set; }
    public string? Github { get; set; }
    public string? Medium { get; set; }

    public UpdateCommunityCommand(int id, string name, string? description, string? tags, string? location, string? organizators, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        Id = id;
        Name = name;
        Description = description;
        Tags = tags;
        Location = location;
        Organizators = organizators;
        Website = website;
        Facebook = facebook;
        Twitter = twitter;
        Instagram = instagram;
        Github = github;
        Medium = medium;
    }
}

public class UpdateCommunityCommandHandler : IYtRequestHandler<UpdateCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly ConnectionFactory _rabbitMqFactory;

    public UpdateCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.CommunityRepository.GetAsync(request.Id, cancellationToken);

        if (existingCommunity == null)
        {
            throw new Exception("Community does not exist");
        }

        existingCommunity.SetDate();

        _mapper.Map(request, existingCommunity);

        _unitOfWork.CommunityRepository.Update(existingCommunity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "update_community_queue", $"Community with Id: {existingCommunity.Id}  has been edited.");

        return true;
    }
}
