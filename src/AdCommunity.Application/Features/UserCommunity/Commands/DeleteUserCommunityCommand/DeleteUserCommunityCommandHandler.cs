using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using  AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.UserCommunity.Commands.DeleteUserCommunityCommand;

public class DeleteUserCommunityCommandHandler : IYtRequestHandler<DeleteUserCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService;
    public DeleteUserCommunityCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }

    public async Task<bool> Handle(DeleteUserCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingUserCommunity = await _unitOfWork.GetRepository<UserCommunityRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUserCommunity is null)
            throw new NotExistException(_localizationService, "UserCommunity");

        var community= await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(existingUserCommunity.CommunityId, null, cancellationToken);

        if(community is null)
            throw new NotExistException(_localizationService, "Community");

        community.RemoveUserCommunity(existingUserCommunity);

        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("delete_userCommunity_queue", $"The UserCommunity with ID: {request.Id} has been removed.");

        return true;
    }
}