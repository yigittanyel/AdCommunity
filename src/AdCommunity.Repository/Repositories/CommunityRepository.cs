﻿using AdCommunity.Domain.Repository;
using AdCommunity.Domain.Entities.Aggregates.Community;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class CommunityRepository : GenericRepository<Community>, ICommunityRepository
{
    public CommunityRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Community>> GetAllWithIncludeAsync(CancellationToken? cancellationToken)
    {
        var communities = await _dbContext.Communities
                                            .Include(x => x.User)
                                            .AsNoTracking()
                                            .ToListAsync((CancellationToken)cancellationToken);
        return communities;
        
    }

    public async Task<Community> GetByCommunityNameAsync(string communityName, CancellationToken? cancellationToken)
    {
        var community= await _dbContext.Communities
                                        .AsNoTracking()
                                        .FirstOrDefaultAsync(x => x.Name == communityName, (CancellationToken)(cancellationToken));
        return community;
    }
}
