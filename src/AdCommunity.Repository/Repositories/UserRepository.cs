using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<User> GetUsersByUsernameAndPasswordAsync(string username, string password , CancellationToken cancellationToken)
    {
        return await _dbContext.Users
                .Where(u => u.Username == username && u.Password == password)
                .AsNoTracking()
                .FirstOrDefaultAsync(cancellationToken);
    }
}
