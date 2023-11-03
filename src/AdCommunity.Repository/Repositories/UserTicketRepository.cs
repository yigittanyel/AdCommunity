using AdCommunity.Domain.Repository;
using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Repository.Context;

namespace AdCommunity.Repository.Repositories;

public class UserTicketRepository : GenericRepository<UserTicket>, IUserTicketRepository
{
    public UserTicketRepository(ApplicationDbContext context) : base(context)
    {
    }
}
