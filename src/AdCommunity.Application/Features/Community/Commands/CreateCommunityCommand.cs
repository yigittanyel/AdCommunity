using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Helpers;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using FluentValidation;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Community.Commands;

public class CreateCommunityCommand:IYtRequest<CommunityCreateDto>
{
    public CommunityCreateDto Community { get; set; }
}

public class CreateCommunityCommandHandler : IYtRequestHandler<CreateCommunityCommand, CommunityCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly ConnectionFactory _rabbitMqFactory;

    public CreateCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<CommunityCreateDto> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.CommunityRepository.GetByCommunityNameAsync(request.Community.Name,cancellationToken);

        if(existingCommunity is not null)
        {
            throw new Exception("Community already exists");
        }

        var community = new AdCommunity.Domain.Entities.Aggregates.Community.Community(request.Community.Name, request.Community.Description, request.Community.Tags, request.Community.Location, request.Community.Organizators, request.Community.Website, request.Community.Facebook, request.Community.Twitter, request.Community.Instagram, request.Community.Github, request.Community.Medium);

        var validationResult = await new CommunityCreateDtoValidator().ValidateAsync(request.Community);

        if (!validationResult.IsValid)
        {
            throw new ValidationException(validationResult.Errors);
        }


        await _unitOfWork.CommunityRepository.AddAsync(community, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "create_community_queue", $"Community name: {community.Name} has been created.");

        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.Community.Community, CommunityCreateDto>(community);
    }
}
