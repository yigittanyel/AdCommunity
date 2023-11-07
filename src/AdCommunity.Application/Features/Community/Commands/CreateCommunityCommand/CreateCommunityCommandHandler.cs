using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Community.Commands.CreateCommunityCommand;

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
        var existingCommunity = await _unitOfWork.CommunityRepository.GetByCommunityNameAsync(request.Name, cancellationToken);

        if (existingCommunity is not null)
            throw new Exception("Community already exists");

        var community = new Domain.Entities.Aggregates.Community.Community(request.Name, request.Description, request.Tags, request.Location, request.UserId, request.Website, request.Facebook, request.Twitter, request.Instagram, request.Github, request.Medium);

        var user = await _unitOfWork.UserRepository.GetAsync(request.UserId, cancellationToken);

        if (user is null)
            throw new Exception("User does not exist");

        await _unitOfWork.CommunityRepository.AddAsync(community, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_community_queue", $"Community name: {community.Name} has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.Community.Community, CommunityCreateDto>(community);
    }
}
