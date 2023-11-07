using AdCommunity.Application.DTOs.User;
using AdCommunity.Core.CustomMediator.Interfaces;

namespace AdCommunity.Application.Features.User.Commands.CreateUserCommand;

public class CreateUserCommand : IYtRequest<UserCreateDto>
{
    public string FirstName { get; set; }
    public string Password { get; set; }
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

    public CreateUserCommand(string firstName, string password, string lastName, string email, string phone, string username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        FirstName = firstName;
        Password = password;
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
    }
}
