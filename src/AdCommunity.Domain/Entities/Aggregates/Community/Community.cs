﻿using AdCommunity.Domain.Entities.Aggregates.User;
using AdCommunity.Domain.Entities.Base;
using System.Text.Json.Serialization;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public partial class Community:BaseEntity,IAggregateRoot
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
        if (user == null)
        {
            throw new ArgumentNullException(nameof(user));
        }

        UserId = user.Id;
        User = user;
    }
    public void SetDate()
    {
        CreatedOn = DateTime.UtcNow;
    }
}
