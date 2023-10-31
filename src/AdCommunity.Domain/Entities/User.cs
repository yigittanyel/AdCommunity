namespace AdCommunity.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public int? SocialId { get; set; }

    public virtual Social? Social { get; set; }

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();

    public virtual ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();
}
