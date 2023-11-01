using AdCommunity.Domain.Entities.UserModels;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }
}
