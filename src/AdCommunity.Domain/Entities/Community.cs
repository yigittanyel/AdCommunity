namespace AdCommunity.Domain.Entities;

public partial class Community
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Tags { get; set; }

    public string? Location { get; set; }

    public string? Organizators { get; set; }

    public int? SocialId { get; set; }

    public virtual ICollection<CommunityEvent> CommunityEvents { get; set; } = new List<CommunityEvent>();

    public virtual Social? Social { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();
}
