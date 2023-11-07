using AdCommunity.Application.DTOs.User;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.User.Queries.GetUsersQuery;

public class GetUsersQuery : IYtRequest<List<UserDto>>
{
}
