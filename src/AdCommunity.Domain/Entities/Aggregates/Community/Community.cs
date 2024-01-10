using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class Community : BaseEntity, IAggregateRoot
{
    public string Name { get; protected set; } = null!;
    public string? Description { get; protected set; }
    public string? Tags { get; protected set; }
    public string? Location { get; protected set; }
    public string? Website { get; protected set; }
    public string? Facebook { get; protected set; }
    public string? Twitter { get; protected set; }
    public string? Instagram { get; protected set; }
    public string? Github { get; protected set; }
    public string? Medium { get; protected set; }
    public int UserId { get; protected set; }
    public virtual ICollection<Event> Events { get; protected set; } = new List<Event>();
    public virtual ICollection<TicketType> Tickets { get; protected set; } = new List<TicketType>();
    public virtual AdCommunity.Domain.Entities.Aggregates.User.User User { get; protected set; } = null!;
    public virtual ICollection<UserCommunity> UserCommunities { get; protected set; } = new List<UserCommunity>();

    public Community()
    {       
    }
    public Community(string name, string? description, string? tags, string? location, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        ArgumentException.ThrowIfNullOrEmpty(name, nameof(name));

        Name = name;
        Description = description;
        Tags = tags;
        Location = location;
        Website = website;
        Facebook = facebook;
        Twitter = twitter;
        Instagram = instagram;
        Github = github;
        Medium = medium;
        CreatedOn = DateTime.UtcNow;
    }
    public void AssignUser(AdCommunity.Domain.Entities.Aggregates.User.User user)
    {
        ArgumentException.ThrowIfNullOrEmpty(nameof(user));

        User = user;
        UserId = user.Id;
    }

    public void AddEvent(Event @event)
    {
        if (@event is null)
            throw new ArgumentNullException(nameof(@event));

        if (!Events.Contains(@event))
            Events.Add(@event);
    }

    public void AddTicket(TicketType ticket)
    {
        if (ticket is null)
            throw new ArgumentNullException(nameof(ticket));

        if (!Tickets.Contains(ticket))
            Tickets.Add(ticket);
    }

    public void AddUserCommunity(UserCommunity userCommunity)
    {
        if (userCommunity is null)
            throw new ArgumentNullException(nameof(userCommunity));

        if (!UserCommunities.Contains(userCommunity))
            UserCommunities.Add(userCommunity);
    }

    public void RemoveEvent(Event @event)
    {
        if (@event is null)
            throw new ArgumentNullException(nameof(@event));

        if (Events.Contains(@event))
            Events.Remove(@event);
    }

    public void RemoveTicket(TicketType ticket)
    {
        if (ticket is null)
            throw new ArgumentNullException(nameof(ticket));

        if (Tickets.Contains(ticket))
            Tickets.Remove(ticket);
    }

    public void RemoveUserCommunity(UserCommunity userCommunity)
    {
        if (userCommunity is null)
            throw new ArgumentNullException(nameof(userCommunity));

        if (UserCommunities.Contains(userCommunity))
            UserCommunities.Remove(userCommunity);
    }

    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }
}
