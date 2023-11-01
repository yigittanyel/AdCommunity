using AdCommunity.Application.Features.User.Responses;
using AdCommunity.Core.CustomMapper;
using AdCommunity.Core.Extensions.Query;
using AdCommunity.Repository.Contracts;

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
            return _ytMapper.Map<Domain.Entities.UserModels.User,GetUsersResponse>((Domain.Entities.UserModels.User)users);
        }
    }
}
