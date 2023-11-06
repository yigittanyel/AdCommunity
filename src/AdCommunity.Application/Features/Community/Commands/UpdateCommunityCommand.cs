using AdCommunity.Application.DTOs.Community;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using RabbitMQ.Client;

namespace AdCommunity.Application.Features.Community.Commands;

public class UpdateCommunityCommand:IYtRequest<bool>
{
    public CommunityUpdateDto Community { get; set; }
}

public class UpdateCommunityCommandHandler : IYtRequestHandler<UpdateCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly ConnectionFactory _rabbitMqFactory;

    public UpdateCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, ConnectionFactory rabbitMqFactory)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
    }

    public async Task<bool> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.CommunityRepository.GetAsync(request.Community.Id, cancellationToken);

        if (existingCommunity == null)
        {
            throw new Exception("Community does not exist");
        }

        existingCommunity.SetDate();

        _mapper.Map(request.Community, existingCommunity);

        _unitOfWork.CommunityRepository.Update(existingCommunity);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        Helpers.MessageBrokerHelper.PublishMessage(_rabbitMqFactory, "update_community_queue", $"Community with Id: {existingCommunity.Id}  has been edited.");

        return true;
    }
}
