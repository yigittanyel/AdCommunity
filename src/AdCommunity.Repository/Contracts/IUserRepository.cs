using AdCommunity.Domain.Entities.UserModels;

namespace AdCommunity.Repository.Contracts;

public interface IUserRepository : IGenericRepository<User>
{
    Task<IEnumerable<User>> GetUsersByUsernameAndPasswordAsync(string username, string password);
}
