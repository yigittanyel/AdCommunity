using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Repository;
using AdCommunity.Repository.Context;
using Microsoft.EntityFrameworkCore;

namespace AdCommunity.Repository.Repositories;

public class UserTicketRepository : GenericRepository<UserTicket>, IUserTicketRepository
{
    public UserTicketRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<UserTicket> GetUserTicketsByUserAndTicketAsync(int userId, int ticketId, CancellationToken? cancellationToken)
    {
        return await _dbContext.UserTickets
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.UserId == userId && x.TicketId == ticketId, 
            cancellationToken ?? CancellationToken.None);
    }
}
