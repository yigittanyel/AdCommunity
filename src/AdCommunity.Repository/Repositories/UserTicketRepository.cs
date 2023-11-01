using AdCommunity.Domain.Entities.UserModels;
using AdCommunity.Repository.Context;
using AdCommunity.Repository.Contracts;

namespace AdCommunity.Repository.Repositories;

public class UserTicketRepository : GenericRepository<UserTicket>, IUserTicketRepository
{
    public UserTicketRepository(ApplicationDbContext context) : base(context)
    {
    }
}
