using AdCommunity.Domain.Entities.SharedKernel;
using System.Security.Cryptography;
using System.Text;

namespace AdCommunity.Domain.Entities.Aggregates.User;


public partial class User: BaseEntity,IAggregateRoot
{
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
    public string? HashedPassword { get; protected set; }
    public virtual ICollection<AdCommunity.Domain.Entities.Aggregates.Community.Community> Communities { get; protected set; } = new List<AdCommunity.Domain.Entities.Aggregates.Community.Community>();
    public virtual ICollection<UserCommunity> UserCommunities { get; protected set; } = new List<UserCommunity>();
    public virtual ICollection<UserEvent> UserEvents { get; protected set; } = new List<UserEvent>();
    public virtual ICollection<UserTicket> UserTickets { get; protected set; } = new List<UserTicket>();
    public User(string firstName, string lastName, string email, string password, string phone, string username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        ArgumentException.ThrowIfNullOrEmpty(firstName, nameof(firstName));
        ArgumentException.ThrowIfNullOrEmpty(lastName, nameof(lastName));
        ArgumentException.ThrowIfNullOrEmpty(email, nameof(email));
        ArgumentException.ThrowIfNullOrEmpty(password, nameof(password));
        ArgumentException.ThrowIfNullOrEmpty(phone, nameof(phone));
        ArgumentException.ThrowIfNullOrEmpty(username, nameof(username));

        if (IsValidEmail(email) == false)
        {
            throw new ArgumentException("This email address is not valid.", nameof(email));
        }

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

    public bool IsValidEmail(string email)
    {
        try
        {
            var addr = new System.Net.Mail.MailAddress(email);
            return addr.Address == email;
        }
        catch
        {
            return false;
        }
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

    public static User Create(string firstName, string lastName, string email, string password, string phone, string username, string? website, string? facebook, string? twitter, string? instagram, string? github, string? medium)
    {
        return new User(firstName, lastName, email, password, phone, username, website, facebook, twitter, instagram, github, medium);
    }

    public void AddCommunity(AdCommunity.Domain.Entities.Aggregates.Community.Community community)
    {
        if (community == null)
            throw new ArgumentNullException(nameof(community));

        if (!Communities.Contains(community))
            Communities.Add(community);
    }

    public void RemoveCommunity(AdCommunity.Domain.Entities.Aggregates.Community.Community community)
    {
        if (community == null)
            throw new ArgumentNullException(nameof(community));

        if (Communities.Contains(community))
            Communities.Remove(community);
    }

    public void AddUserCommunity(UserCommunity userCommunity)
    {
        if (userCommunity == null)
            throw new ArgumentNullException(nameof(userCommunity));

        if (!UserCommunities.Contains(userCommunity))
            UserCommunities.Add(userCommunity);
    }

    public void RemoveUserCommunity(UserCommunity userCommunity)
    {
        if (userCommunity == null)
            throw new ArgumentNullException(nameof(userCommunity));

        if (UserCommunities.Contains(userCommunity))
            UserCommunities.Remove(userCommunity);
    }

    public void AddUserEvent(UserEvent userEvent)
    {
        if (userEvent == null)
            throw new ArgumentNullException(nameof(userEvent));

        if (!UserEvents.Contains(userEvent))
            UserEvents.Add(userEvent);
    }

    public void RemoveUserEvent(UserEvent userEvent)
    {
        if (userEvent == null)
            throw new ArgumentNullException(nameof(userEvent));

        if (UserEvents.Contains(userEvent))
            UserEvents.Remove(userEvent);
    }

    public void AddUserTicket(UserTicket userTicket)
    {
        if (userTicket == null)
            throw new ArgumentNullException(nameof(userTicket));

        if (!UserTickets.Contains(userTicket))
            UserTickets.Add(userTicket);
    }

    public void RemoveUserTicket(UserTicket userTicket)
    {
        if (userTicket == null)
            throw new ArgumentNullException(nameof(userTicket));

        if (UserTickets.Contains(userTicket))
            UserTickets.Remove(userTicket);
    }
}
