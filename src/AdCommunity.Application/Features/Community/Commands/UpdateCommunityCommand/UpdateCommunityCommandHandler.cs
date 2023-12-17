using AdCommunity.Application.Exceptions;
using AdCommunity.Application.Services.RabbitMQ;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Repositories;
using Microsoft.AspNetCore.Http;

namespace AdCommunity.Application.Features.Community.Commands.UpdateCommunityCommand;

public class UpdateCommunityCommandHandler : IYtRequestHandler<UpdateCommunityCommand, bool>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;
    private readonly IMessageBrokerService _rabbitMqFactory;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UpdateCommunityCommandHandler(IUnitOfWork unitOfWork, IYtMapper mapper, IMessageBrokerService rabbitMqFactory, IHttpContextAccessor httpContextAccessor)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
        _rabbitMqFactory = rabbitMqFactory;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task<bool> Handle(UpdateCommunityCommand request, CancellationToken cancellationToken)
    {
        var existingCommunity = await _unitOfWork.GetRepository<CommunityRepository>().GetAsync(request.Id, null, cancellationToken);

        if (existingCommunity is null)
            throw new NotExistException("Community",_httpContextAccessor.HttpContext);

        existingCommunity.SetDate();

        var user= await _unitOfWork.GetRepository<UserRepository>().GetAsync(request.UserId, null, cancellationToken);
        
        if (user is null)
            throw new NotExistException("User",_httpContextAccessor.HttpContext);

        existingCommunity.AssignUser(user);

        _mapper.Map(request, existingCommunity);

        _unitOfWork.GetRepository<CommunityRepository>().Update(existingCommunity);

        _rabbitMqFactory.PublishMessage("update_community_queue", $"Community with Id: {existingCommunity.Id}  has been edited.");

        return true;
    }
}
