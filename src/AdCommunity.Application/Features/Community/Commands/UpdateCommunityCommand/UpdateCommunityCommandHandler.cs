using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.Community.Commands.UpdateCommunityCommand;

public class UpdateCommunityCommandHandler : IYtRequestHandler<UpdateCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;

    public UpdateCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.CommunityRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingCommunity == null)
            throw new Exception("Community does not exist");

        existingCommunity.SetDate();

        var user= await _unitOfWork.UserRepository.GetAsync(request.UserId, null, cancellationToken);
        existingCommunity.AssignUser(user);

        _mapper.Map(request, existingCommunity);

        _unitOfWork.CommunityRepository.Update(existingCommunity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        _rabbitMqFactory.PublishMessage("update_community_queue", $"Community with Id: {existingCommunity.Id}  has been edited.");

        return true;
    }
}
