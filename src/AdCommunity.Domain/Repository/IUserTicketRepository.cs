using AdCommunity.Domain.Entities.Aggregates.User;

namespace AdCommunity.Domain.Repository;

public interface IUserTicketRepository : IGenericRepository<UserTicket>
{
    Task<UserTicket> GetUserTicketsByUserAndTicketAsync(int userId, int ticketId, CancellationToken? cancellationToken);
}
