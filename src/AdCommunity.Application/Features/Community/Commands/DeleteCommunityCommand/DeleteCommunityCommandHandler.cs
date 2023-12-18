using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Core.Helpers;
using AdCommunity.Core.UnitOfWork;
using AdCommunity.Repository.Repositories;

namespace AdCommunity.Application.Features.Community.Commands.DeleteCommunityCommand;
public class DeleteCommunityCommandHandler : IYtRequestHandler<DeleteCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly LocalizationService _localizationService;
    public DeleteCommunityCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, LocalizationService localizationService)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _localizationService = localizationService;
    }

    public async Task<bool> Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingCommunity is null)
            throw new NotExistException(_localizationService,"Community");

        _unitOfWork.GetRepository<CommunityRepository>().Delete(existingCommunity);

        _rabbitMqFactory.PublishMessage("delete_community_queue", $"Community name: {existingCommunity.Name} has been removed.");

        return true;
    }
}
