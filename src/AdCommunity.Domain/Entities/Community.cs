namespace AdCommunity.Domain.Entities;

public partial class Community
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? Description { get; set; }

    public string? Tags { get; set; }

    public string? Location { get; set; }

    public string? Organizators { get; set; }

    public string? Social { get; set; }

    public int? Membercount { get; set; }

    public virtual ICollection<Communityevent> Communityevents { get; set; } = new List<Communityevent>();

    public virtual ICollection<Usercommunity> Usercommunities { get; set; } = new List<Usercommunity>();
}
