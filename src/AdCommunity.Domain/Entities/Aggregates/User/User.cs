using AdCommunity.Domain.Exceptions;

namespace AdCommunity.Domain.Entities.Aggregates.User;


public partial class User
{
    public int Id { get; protected set; }

    public string FirstName { get; protected set; } = null!;

    public string LastName { get; protected set; } = null!;

    public string Email { get; protected set; } = null!;

    public string Password { get; protected set; } = null!;

    public string Phone { get; protected set; } = null!;

    public string Username { get; protected set; } = null!;

    public string? Website { get; protected set; }

    public string? Facebook { get; protected set; }

    public string? Twitter { get; protected set; }

    public string? Instagram { get; protected set; }

    public string? Github { get; protected set; }

    public string? Medium { get; protected set; }

    public DateTime? CreatedOn { get; protected set; }

    public string? HashedPassword_ { get; protected set; }

    public virtual ICollection<UserCommunity> UserCommunities { get; set; } = new List<UserCommunity>();

    public virtual ICollection<UserEvent> UserEvents { get; set; } = new List<UserEvent>();

    public virtual ICollection<UserTicket> UserTickets { get; set; } = new List<UserTicket>();

    public User(string firstName, string lastName, string email, string password, string phone, string username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        if (string.IsNullOrEmpty(firstName))
            throw new NullException(nameof(firstName));
        if (string.IsNullOrEmpty(lastName))
            throw new NullException(nameof(lastName));
        if (string.IsNullOrEmpty(email))
            throw new NullException(nameof(email));
        if (string.IsNullOrEmpty(password))
            throw new NullException(nameof(password));
        if (string.IsNullOrEmpty(phone))
            throw new NullException(nameof(phone));
        if (string.IsNullOrEmpty(username))
            throw new NullException(nameof(username));

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
        CreatedOn = DateTime.UtcNow;
    }
}
