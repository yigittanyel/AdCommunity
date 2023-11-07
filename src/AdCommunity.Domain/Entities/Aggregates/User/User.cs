using AdCommunity.Domain.Exceptions;
using System.Security.Cryptography;
using System.Text;

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

    public string? HashedPassword { get; protected set; }

    public virtual ICollection<AdCommunity.Domain.Entities.Aggregates.Community.Community> Communities { get; set; } = new List<AdCommunity.Domain.Entities.Aggregates.Community.Community>();

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
        HashedPassword = HashPassword(password);
    }

    public void SetHashedPassword(string password)
    {
        HashedPassword = HashPassword(password);
    }

    public void SetDate()
    {
        CreatedOn= DateTime.UtcNow;
    }

    public string HashPassword(string password)
    {
        if (string.IsNullOrEmpty(password))
            throw new ArgumentException("Password cannot be null or empty.");

        using (SHA256 sha256 = SHA256.Create())
        {
            byte[] hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));

            StringBuilder builder = new StringBuilder();
            for (int i = 0; i < hashedBytes.Length; i++)
            {
                builder.Append(hashedBytes[i].ToString("x2"));
            }

            return builder.ToString();
        }
    }
}
