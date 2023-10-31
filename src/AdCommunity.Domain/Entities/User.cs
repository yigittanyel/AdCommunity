namespace AdCommunity.Domain.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Lastname { get; set; }

    public string? Email { get; set; }

    public string? Password { get; set; }

    public string? Phone { get; set; }

    public string? Username { get; set; }

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<Usercommunity> Usercommunities { get; set; } = new List<Usercommunity>();

    public virtual ICollection<Userevent> Userevents { get; set; } = new List<Userevent>();
}
