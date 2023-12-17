using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.UserCommunity.Commands.DeleteUserCommunityCommand;

public class DeleteUserCommunityCommandHandler : IYtRequestHandler<DeleteUserCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DeleteUserCommunityCommandHandler(IUnitOfWork unitOfWork, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(DeleteUserCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingUserCommunity = await _unitOfWork.GetRepository<UserCommunityRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingUserCommunity is null)
            throw new NotExistException("UserCommunity", _httpContextAccessor.HttpContext);

        var community= await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(existingUserCommunity.CommunityId, null, cancellationToken);

        if(community is null)
            throw new NotExistException("Community", _httpContextAccessor.HttpContext);

        community.RemoveUserCommunity(existingUserCommunity);

        _unitOfWork.GetRepository<CommunityRepository>().Update(community);

        _rabbitMqFactory.PublishMessage("delete_userCommunity_queue", $"The UserCommunity with ID: {request.Id} has been removed.");

        return true;
    }
}