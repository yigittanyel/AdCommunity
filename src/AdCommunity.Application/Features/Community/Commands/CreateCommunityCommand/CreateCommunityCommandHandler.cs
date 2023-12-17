using AdCommunity.Application.DTOs.Community;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.Community.Commands.CreateCommunityCommand;

public class CreateCommunityCommandHandler : IYtRequestHandler<CreateCommunityCommand, CommunityCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<CommunityCreateDto> Handle(CreateCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.GetRepository<CommunityRepository>().GetByCommunityNameAsync(request.Name, cancellationToken);

        if (existingCommunity is not null)
            throw new AlreadyExistsException(existingCommunity.Name, _httpContextAccessor.HttpContext);

        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User",_httpContextAccessor.HttpContext);

        var community = new Domain.Entities.Aggregates.Community.Community(request.Name, request.Description, request.Tags, request.Location, request.Website, request.Facebook, request.Twitter, request.Instagram, request.Github, request.Medium);

        community.AssignUser(user);

        //var validationResult = await new CreateCommunityCommandValidator().ValidateAsync(request);

        //if (!validationResults.IsValid)
        //{
        //    throw new ValidationException(validationResults.Errors.Select(e => e.ErrorMessage).ToList());
        //}

        await _unitOfWork.GetRepository<CommunityRepository>().AddAsync(community, cancellationToken);


        _rabbitMqFactory.PublishMessage("create_community_queue", $"Community name: {community.Name} has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.Community.Community, CommunityCreateDto>(community);
    }
}
