namespace AdCommunity.Domain.Entities;

public partial class Social
{
    public int Id { get; set; }

    public string? website { get; set; }

    public string? Facebook { get; set; }

    public string? Twitter { get; set; }

    public string? Instagram { get; set; }

    public string? Twitch { get; set; }

    public string? Github { get; set; }

    public string? Medium { get; set; }

    public virtual ICollection<Community> Communities { get; set; } = new List<Community>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
