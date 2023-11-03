using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.UnitOfWork;
using System.Threading;

namespace AdCommunity.Application.Features.User.Queries;

public class GetUsersQuery : IYtRequest<List<UserDto>> 
{ 
}

public class GetUsersQueryHandler : IYtRequestHandler<GetUsersQuery, List<UserDto>>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;

    public GetUsersQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<List<UserDto>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var users = await _unitOfWork.UserRepository.GetAllAsync(cancellationToken);

        if (users == null || !users.Any())
        {
            throw new NotFoundException("User");
        }

        var userDto = _mapper.MapList<AdCommunity.Domain.Entities.Aggregates.User.User, UserDto>((List<Domain.Entities.Aggregates.User.User>)users);

        return userDto;
    }
}
