using AdCommunity.Application.DTOs.Community;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Community.Commands.CreateCommunityCommand;

public class CreateCommunityCommand : IYtRequest<CommunityCreateDto>
{
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? Tags { get; set; }
    public string? Location { get; set; }
    public string? Organizators { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Instagram { get; set; }
    public string? Github { get; set; }
    public string? Medium { get; set; }

    public CreateCommunityCommand(string name, string? description, string? tags, string? location, string? organizators, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
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
    }
}
