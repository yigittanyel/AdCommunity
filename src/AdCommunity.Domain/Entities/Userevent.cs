using System.ComponentModel.DataAnnotations.Schema;

namespace AdCommunity.Domain.Entities;

public partial class Userevent
{
    public int Id { get; set; }

    public int? Userid { get; set; }

    public int? Eventid { get; set; }

    [ForeignKey(nameof(Eventid))]

    public virtual Communityevent? Event { get; set; }

    [ForeignKey(nameof(Userid))]

    public virtual User? User { get; set; }
}
