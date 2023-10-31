namespace AdCommunity.Domain.Entities;

public partial class Ticket
{
    public int Id { get; set; }

    public string? Pnr { get; set; }

    public int? CommunityEventId { get; set; }

    public int? CommunityId { get; set; }

    public decimal? Price { get; set; }

    public virtual Community? Community { get; set; }

    public virtual CommunityEvent? CommunityEvent { get; set; }

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
