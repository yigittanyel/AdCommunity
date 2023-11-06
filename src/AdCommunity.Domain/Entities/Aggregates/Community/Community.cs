using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class Community
{
    public int Id { get; protected set; }

    public string Name { get; protected set; } = null!;

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

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();

    public Community(string name, string? description, string? tags, string? location, string? organizators, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        if (string.IsNullOrEmpty(name))
            throw new NullException(nameof(name));

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
        CreatedOn= DateTime.UtcNow;
    }

    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }
}
