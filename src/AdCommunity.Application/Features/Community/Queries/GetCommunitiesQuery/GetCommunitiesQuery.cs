using AdCommunity.Application.DTOs.Community;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Community.Queries.GetCommunitiesQuery;

public class GetCommunitiesQuery : IYtRequest<List<CommunityDto>>
{
    public bool IsCommand => false;

}
