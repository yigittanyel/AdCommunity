using AdCommunity.Application.DTOs.User;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.User.Queries.GetUserQuery;

public class GetUserQuery : IYtRequest<UserDto>
{
    public bool IsCommand => false;
    public int Id { get; set; }
}
