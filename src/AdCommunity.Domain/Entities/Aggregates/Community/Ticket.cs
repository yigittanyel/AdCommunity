using AdCommunity.Domain.Entities.Aggregates.User;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class Ticket
{
    public int Id { get; protected set; }

    public int CommunityEventId { get; protected set; }

    public int CommunityId { get; protected set; }

    public decimal? Price { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

    public virtual Community Community { get; protected set; } = null!;

    public virtual Event CommunityEvent { get; protected set; } = null!;

    public virtual ICollection<UserTicket> UserTickets { get; protected set; } = new List<UserTicket>();


}
