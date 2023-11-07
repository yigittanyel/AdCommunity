using AdCommunity.Application.DTOs.Community;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Community.Queries.GetCommunityQuery;

public class GetCommunityQuery : IYtRequest<CommunityDto>
{
    public int Id { get; set; }
}
