using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Helpers;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Commands;

public class CreateCommunityCommand:IYtRequest<CommunityCreateDto>
{
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

    public CreateCommunityCommand(string name, string? description, string? tags, string? location, string? organizators, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
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

public class CreateCommunityCommandHandler : IYtRequestHandler<CreateCommunityCommand, CommunityCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public CreateCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<CommunityCreateDto> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.CommunityRepository.GetByCommunityNameAsync(request.Name,cancellationToken);

        if(existingCommunity is not null)
        {
            throw new Exception("Community already exists");
        }

        var community = new AdCommunity.Domain.Entities.Aggregates.Community.Community(request.Name, request.Description, request.Tags, request.Location, request.Organizators, request.Website, request.Facebook, request.Twitter, request.Instagram, request.Github, request.Medium);

        await _unitOfWork.CommunityRepository.AddAsync(community, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_community_queue", $"Community name: {community.Name} has been created.");

        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.Community.Community, CommunityCreateDto>(community);
    }
}
