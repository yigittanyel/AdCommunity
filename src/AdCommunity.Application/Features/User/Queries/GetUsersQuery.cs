using AdCommunity.Application.Features.User.Responses;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.Extensions.Query;
using AdCommunity.Domain.Contracts;

namespace AdCommunity.Application.Features.User.Queries;

public class GetUsersQuery : IYtQuery<GetUsersResponse>
{
    public class GetUsersQueryHandler : IYtQueryHandler<GetUsersQuery, GetUsersResponse>
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IYtMapper _ytMapper;

        public GetUsersQueryHandler(IUnitOfWork unitOfWork, IYtMapper ytMapper)
        {
            _unitOfWork = unitOfWork;
            _ytMapper = ytMapper;
        }

        public async Task<GetUsersResponse> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            return _ytMapper.Map<Domain.Entities.Aggregates.User.User,GetUsersResponse>((Domain.Entities.Aggregates.User.User)users);
        }
    }
}
