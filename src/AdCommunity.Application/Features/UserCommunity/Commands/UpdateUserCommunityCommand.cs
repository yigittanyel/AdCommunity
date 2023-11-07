using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserCommunity.Commands;

public class UpdateUserCommunityCommand:IYtRequest<bool>
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int CommunityId { get; set; }
    public DateTime? JoinDate { get; set; }

    public UpdateUserCommunityCommand(int id, int userId, int communityId, DateTime? joinDate)
    {
        Id = id;
        UserId = userId;
        CommunityId = communityId;
        JoinDate = joinDate;
    }
}

public class UpdateUserCommunityCommandHandler : IYtRequestHandler<UpdateUserCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public UpdateUserCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateUserCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingUserCommunity = await _unitOfWork.UserCommunityRepository.GetAsync(request.Id, cancellationToken);

        if (existingUserCommunity == null)
        {
            throw new Exception("UserCommunity does not exist");
        }

        existingUserCommunity.SetDate();

        _mapper.Map(request, existingUserCommunity);

        _unitOfWork.UserCommunityRepository.Update(existingUserCommunity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("update_userCommunity_queue", $"UserCommunity with Id: {existingUserCommunity.Id}  has been edited.");

        return true;
    }
}