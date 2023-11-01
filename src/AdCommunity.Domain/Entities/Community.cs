namespace AdCommunity.Domain.Entities;

public partial class Community
{
    public int Id { get; protected set; }

    public string? Name { get; protected set; }

    public string? Description { get; protected set; }

    public string? Tags { get; protected set; }

    public string? Location { get; protected set; }

    public string? Organizators { get; protected set; }

    public string? Website { get; protected set; }

    public string? Facebook { get; protected set; }

    public string? Twitter { get; protected set; }

    public string? Instagram { get; protected set; }

    public string? Github { get; protected set; }

    public string? Medium { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

    public virtual ICollection<Event> Events { get; protected set; } = new List<Event>();

    public virtual ICollection<Ticket> Tickets { get; protected set; } = new List<Ticket>();

    public virtual ICollection<UserCommunity> UserCommunities { get; protected set; } = new List<UserCommunity>();

    public Community(int id, string? name, string? description, string? tags, string? location, string? organizators, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium, DateTime? createdOn, ICollection<Event> events, ICollection<Ticket> tickets, ICollection<UserCommunity> userCommunities)
    {
        Id = id;
        Name = name;
        Description = description;
        Tags = tags;
        Location = location;
        Organizators = organizators;
        Website = website;
        Facebook = facebook;
        Twitter = twitter;
        Instagram = instagram;
        Github = github;
        Medium = medium;
        CreatedOn = createdOn;
        Events = events;
        Tickets = tickets;
        UserCommunities = userCommunities;
    }

    public Community(int id, string? name, string? description, string? tags, string? location, string? organizators, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium, DateTime? createdOn)
    {
        Id = id;
        Name = name;
        Description = description;
        Tags = tags;
        Location = location;
        Organizators = organizators;
        Website = website;
        Facebook = facebook;
        Twitter = twitter;
        Instagram = instagram;
        Github = github;
        Medium = medium;
        CreatedOn = createdOn;
    }

    public Community(int id)
    {
        Id = id;
    }
}
