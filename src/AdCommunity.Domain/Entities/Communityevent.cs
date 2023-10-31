namespace AdCommunity.Domain.Entities;

public partial class CommunityEvent
{
    public int Id { get; set; }

    public string? EventName { get; set; }

    public string? Description { get; set; }

    public DateTime? EventDate { get; set; }

    public string? Location { get; set; }

    public int? CommunityId { get; set; }

    public virtual Community? Community { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();
}
