using System.ComponentModel.DataAnnotations.Schema;

namespace AdCommunity.Domain.Entities;

public partial class Communityevent
{
    public int Id { get; set; }

    public string? Eventname { get; set; }

    public string? Description { get; set; }

    public DateTime? Eventdate { get; set; }

    public string? Location { get; set; }

    public int? Communityid { get; set; }

    public int? Attendingmembercount { get; set; }

    [ForeignKey(nameof(Communityid))]
    public virtual Community? Community { get; set; }

    public virtual ICollection<Userevent> Userevents { get; set; } = new List<Userevent>();
}
