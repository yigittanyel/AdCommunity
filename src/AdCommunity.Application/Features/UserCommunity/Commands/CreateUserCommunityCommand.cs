using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserCommunity.Commands;

public class CreateUserCommunityCommand:IYtRequest<UserCommunityCreateDto>
{
    public int UserId { get; set; }
    public int CommunityId { get; set; }
    public DateTime? JoinDate { get; set; }

    public CreateUserCommunityCommand(int userId, int communityId, DateTime? joinDate)
    {
        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
    }
}

public class CreateUserCommunityCommandHandler : IYtRequestHandler<CreateUserCommunityCommand, UserCommunityCreateDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public CreateUserCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<UserCommunityCreateDto> Handle(CreateUserCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingUserCommunity = await _unitOfWork.UserCommunityRepository.GetUserCommunitiesByUserAndCommunityAsync(request.UserId,request.CommunityId, cancellationToken);

        if (existingUserCommunity is not null)
            throw new Exception("UserCommunity already exists");

        var userCommunity = new AdCommunity.Domain.Entities.Aggregates.User.UserCommunity(request.UserId, request.CommunityId,request.JoinDate); 

        var user= await _unitOfWork.UserRepository.GetAsync(request.UserId, cancellationToken);
        var community= await _unitOfWork.CommunityRepository.GetAsync(request.CommunityId, cancellationToken);

        if (user is null)
            throw new Exception("User does not exist");

        if (community is null)
            throw new Exception("Community does not exist");

        await _unitOfWork.UserCommunityRepository.AddAsync(userCommunity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("create_userCommunity_queue", $"UserCommunity has been created.");

        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.User.UserCommunity, UserCommunityCreateDto>(userCommunity);
    }
}