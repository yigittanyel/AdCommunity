using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.Community.Commands.UpdateCommunityCommand;

public class UpdateCommunityCommand : IYtRequest<bool>
{
    public int Id { get; set; }
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
    public int UserId { get; set; }

    public UpdateCommunityCommand(int id, string name, string? description, string? tags, string? location, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium,int userId)
    {
        Id = id;
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
        UserId = userId;
    }
}
