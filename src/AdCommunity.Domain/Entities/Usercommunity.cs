namespace AdCommunity.Domain.Entities;

public partial class Usercommunity
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public int? Communityid { get; set; }

    public DateTime? Joindate { get; set; }

    public virtual Community? Community { get; set; }

    public virtual User? User { get; set; }
}
