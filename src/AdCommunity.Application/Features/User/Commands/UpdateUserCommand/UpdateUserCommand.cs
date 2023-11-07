using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.User.Commands.UpdateUserCommand;

public class UpdateUserCommand : IYtRequest<bool>
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public string Username { get; set; }
    public string? Website { get; set; }
    public string? Facebook { get; set; }
    public string? Twitter { get; set; }
    public string? Instagram { get; set; }
    public string? Github { get; set; }
    public string? Medium { get; set; }
    public string Password { get; set; }

    public UpdateUserCommand(int id, string firstName, string lastName, string email, string phone, string username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium, string password)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Phone = phone;
        Username = username;
        Website = website;
        Facebook = facebook;
        Twitter = twitter;
        Instagram = instagram;
        Github = github;
        Medium = medium;
        Password = password;
    }
}
