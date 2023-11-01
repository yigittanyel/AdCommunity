namespace AdCommunity.Application.Features.User.Responses;

public class GetUsersResponse
{
    public int Id { get; set; }
    public DateTime CreatedOn { get; set; }
    public string? FirstName { get; protected set; }
    public string? LastName { get; protected set; }
    public string? Email { get; protected set; }
    public string? HashedPassword { get; protected set; }
    public string? Phone { get; protected set; }
    public string? Username { get; protected set; }
    public string? Website { get; protected set; }
    public string? Facebook { get; protected set; }
    public string? Twitter { get; protected set; }
    public string? Instagram { get; protected set; }
    public string? Github { get; protected set; }
    public string? Medium { get; protected set; }
}
