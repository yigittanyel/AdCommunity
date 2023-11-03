using AdCommunity.Application.DTOs.User;
using AdCommunity.Application.Exceptions;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.CustomMediator.Interfaces;
using AdCommunity.Domain.Repository;

namespace AdCommunity.Application.Features.User.Queries;

public class GetUserQuery : IYtRequest<UserDto>
{
    public int Id { get; set; }
}

public class GetUserQueryHandler : IYtRequestHandler<GetUserQuery, UserDto>
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IYtMapper _mapper;

    public GetUserQueryHandler(IUnitOfWork unitOfWork, IYtMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<UserDto> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _unitOfWork.UserRepository.GetAsync(request.Id,cancellationToken);

        if (user == null)
        {
            throw new NotFoundException("User", request.Id);
        }

        var userDto= _mapper.Map<AdCommunity.Domain.Entities.Aggregates.User.User, UserDto>(user);
        return userDto;
    }
}