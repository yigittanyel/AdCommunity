using AdCommunity.Domain.Entities.UserModels;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class UserRepository : GenericRepository<User>,IUserRepository
{
    public UserRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<User>> GetUsersByUsernameAndPasswordAsync(string username, string password)
    {
        return await _dbContext.Users
            .Where(u => u.Username == username && u.Password == password)
            .ToListAsync();
    }
}
