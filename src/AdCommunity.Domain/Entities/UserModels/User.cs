using AdCommunity.Domain.Base;
using System.Text;

namespace AdCommunity.Domain.Entities.UserModels;

public partial class User:BaseEntity,IAggregateRoot
{
    public string? FirstName { get; protected set; }

    public string? LastName { get; protected set; }

    public string? Email { get; protected set; }

    public string? Password { get; protected set; }
    public string? HashedPassword { get; protected set; }

    public string? Phone { get; protected set; }

    public string? Username { get; protected set; }

    public string? Website { get; protected set; }

    public string? Facebook { get; protected set; }

    public string? Twitter { get; protected set; }

    public string? Instagram { get; protected set; }

    public string? Github { get; protected set; }

    public string? Medium { get; protected set; }

    public virtual ICollection<UserCommunity> UserCommunities { get; protected set; } = new List<UserCommunity>();

    public virtual ICollection<UserEvent> UserEvents { get; protected set; } = new List<UserEvent>();

    public virtual ICollection<UserTicket> UserTickets { get; protected set; } = new List<UserTicket>();

    public User(int id, string? firstName, string? lastName, string? email, string? password, string? phone, string? username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium, DateTime? createdOn, ICollection<UserCommunity> userCommunities, ICollection<UserEvent> userEvents, ICollection<UserTicket> userTickets)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Phone = phone;
        Username = username;
        Website = website;
        Facebook = facebook;
        Twitter = twitter;
        Instagram = instagram;
        Github = github;
        Medium = medium;
        CreatedOn = createdOn;
        UserCommunities = userCommunities;
        UserEvents = userEvents;
        UserTickets = userTickets;
    }

    public User(int id, string? firstName, string? lastName, string? email, string? password, string? phone, string? username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium, DateTime? createdOn)
    {
        Id = id;
        FirstName = firstName;
        LastName = lastName;
        Email = email;
        Password = password;
        Phone = phone;
        Username = username;
        Website = website;
        Facebook = facebook;
        Twitter = twitter;
        Instagram = instagram;
        Github = github;
        Medium = medium;
        CreatedOn = createdOn;
    }

    public User(int id)
    {
        Id = id;
    }

    public void SetPassword(string newPassword)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(newPassword));
            HashedPassword = Convert.ToBase64String(hashedBytes);
        }
    }

    public bool VerifyPassword(string passwordToVerify)
    {
        using (var sha256 = System.Security.Cryptography.SHA256.Create())
        {
            var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordToVerify));
            var hashedPasswordToVerify = Convert.ToBase64String(hashedBytes);
            return hashedPasswordToVerify == HashedPassword;
        }
    }
}
