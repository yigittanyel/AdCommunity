using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.UserCommunity.Commands.CreateUserCommunityCommand;

public class CreateUserCommunityCommandHandler : IYtRequestHandler<CreateUserCommunityCommand, UserCommunityCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreateUserCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<UserCommunityCreateDto> Handle(CreateUserCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingUserCommunity = await _unitOfWork.GetRepository<UserCommunityRepository>().GetUserCommunitiesByUserAndCommunityAsync(request.UserId, request.CommunityId, cancellationToken);

        if (existingUserCommunity is not null)
            throw new AlreadyExistsException("User Community", _httpContextAccessor.HttpContext);

        var userCommunity = new Domain.Entities.Aggregates.User.UserCommunity(request.JoinDate);
        var user = await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User",_httpContextAccessor.HttpContext);

        userCommunity.AssignUser(user);

        var community = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.CommunityId, null, cancellationToken);

        if (community is null)
            throw new Exception("Community does not exist");

        userCommunity.AssignCommunity(community);

        community.AddUserCommunity(userCommunity);

        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("create_userCommunity_queue", $"UserCommunity has been created.");

        return _mapper.Map<Domain.Entities.Aggregates.User.UserCommunity, UserCommunityCreateDto>(userCommunity);
    }
}