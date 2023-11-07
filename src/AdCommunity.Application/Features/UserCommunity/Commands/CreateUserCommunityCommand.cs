using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Application.Helpers;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

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
    private readonly ConnectionFactory _rabbitMqFactory;

    public CreateUserCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<UserCommunityCreateDto> Handle(CreateUserCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingUserCommunity = await _unitOfWork.UserCommunityRepository.GetUserCommunitiesByUserAndCommunityAsync(request.UserId,request.CommunityId, cancellationToken);

        if (existingUserCommunity is not null)
        {
            throw new Exception("UserCommunity already exists");
        }

        var userCommunity = new AdCommunity.Domain.Entities.Aggregates.User.UserCommunity(request.UserId, request.CommunityId,request.JoinDate); 

        await _unitOfWork.UserCommunityRepository.AddAsync(userCommunity, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "create_userCommunity_queue", $"UserCommunity has been created.");

        return _mapper.Map<AdCommunity.Domain.Entities.Aggregates.User.UserCommunity, UserCommunityCreateDto>(userCommunity);
    }
}