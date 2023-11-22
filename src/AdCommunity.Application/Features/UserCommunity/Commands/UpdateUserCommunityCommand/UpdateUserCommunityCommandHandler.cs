using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.UserCommunity.Commands.UpdateUserCommunityCommand;

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
        var existingUserCommunity = await _unitOfWork.UserCommunityRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingUserCommunity is null)
            throw new NotExistException("UserCommunity");

        var user = await _unitOfWork.UserRepository.GetAsync(request.UserId, null, cancellationToken);

        if (user is null)
            throw new NotExistException("User");

        existingUserCommunity.AssignUser(user);

        var community = await _unitOfWork.CommunityRepository.GetAsync(request.CommunityId, null, cancellationToken);

        if (community is null)
            throw new NotExistException("Community");

        existingUserCommunity.AssignCommunity(community);

        existingUserCommunity.SetDate();

        _mapper.Map(request, existingUserCommunity);

        _unitOfWork.UserCommunityRepository.Update(existingUserCommunity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("update_userCommunity_queue", $"UserCommunity with Id: {existingUserCommunity.Id}  has been edited.");

        return true;
    }
}