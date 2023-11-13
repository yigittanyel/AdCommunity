﻿using AdCommunity.Application.DTOs.UserCommunity;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.UserCommunity.Queries.GetUserCommunityQuery;

public class GetUserCommunityQuery : IYtRequest<UserCommunityDto>
{
    public int Id { get; set; }
}
