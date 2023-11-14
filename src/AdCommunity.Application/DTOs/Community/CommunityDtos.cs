﻿namespace AdCommunity.Application.DTOs.Community;

public class CommunityBaseDto
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }
    public string? Location { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Instagram { get; set; }
    public string? Github { get; set; }
    public string? Medium { get; set; }
    public AdCommunity.Domain.Entities.Aggregates.User.User User { get; set; }

    public CommunityBaseDto(string name, string? description, string? tags, string? location, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium, AdCommunity.Domain.Entities.Aggregates.User.User communityUser)
    {
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
        User = communityUser;
    }

    public CommunityBaseDto()
    {
    }
}

public class CommunityDto : CommunityBaseDto
{
    public int Id { get; set; }

    public CommunityDto()
    {
    }
}

public class CommunityCreateDto : CommunityBaseDto
{
    public CommunityCreateDto()
    {
    }
}

public class CommunityUpdateDto : CommunityBaseDto
{
    public int Id { get; set; }

    public CommunityUpdateDto()
    {
    }
}
