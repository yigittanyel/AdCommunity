using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class Community
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Description { get; set; }

    public string? Tags { get; set; }

    public string? Location { get; set; }

    public string? Website { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Instagram { get; set; }

    public string? Github { get; set; }

    public string? Medium { get; set; }

    public DateTime? CreatedOn { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Event> Events { get; set; } = new List<Event>();

    public virtual ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();

    public virtual AdCommunity.Domain.Entities.Aggregates.User.User User { get; set; } = null!;

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();

    public Community(string name, string? description, string? tags, string? location, int userId, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        if (string.IsNullOrEmpty(name))
            throw new NullException(nameof(name));

        Name = name;
        Description = description;
        Tags = tags;
        Location = location;
        UserId = userId;
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
