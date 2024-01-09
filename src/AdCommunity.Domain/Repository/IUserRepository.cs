using AdCommunity.Domain.Entities.Aggregates.User;

namespace AdCommunity.Domain.Repository;

public interface IUserRepository : IGenericRepository<User>
{
    Task<User> GetUsersByUsernameAndPasswordAsync(string username, string password,CancellationToken cancellation);
}
