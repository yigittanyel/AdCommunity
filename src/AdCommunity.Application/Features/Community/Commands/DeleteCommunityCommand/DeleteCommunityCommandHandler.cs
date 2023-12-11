using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.Community.Commands.DeleteCommunityCommand;
public class DeleteCommunityCommandHandler : IYtRequestHandler<DeleteCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteCommunityCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.CommunityRepository.GetAsync(request.Id, null, cancellationToken);

        if (existingCommunity is null)
            throw new NotExistException("Community",_httpContextAccessor.HttpContext);

        _unitOfWork.CommunityRepository.Delete(existingCommunity);

        _rabbitMqFactory.PublishMessage("delete_community_queue", $"Community name: {existingCommunity.Name} has been removed.");

        return true;
    }
}
