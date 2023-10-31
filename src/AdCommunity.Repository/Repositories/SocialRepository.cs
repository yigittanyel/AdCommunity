using AdCommunity.Domain.Entities;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;
using AdCommunity.Repository.Repositories;

namespace AdSocial.Repository.Repositories;

public class SocialRepository : GenericRepository<Social>, ISocialRepository
{
    public SocialRepository(ApplicationDbContext context) : base(context)
    {
    }
}
