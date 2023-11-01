using System;
using System.Collections.Generic;

namespace AdCommunity.Domain.Entities;

public partial class UserCommunity
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CommunityId { get; set; }

    public DateTime? JoinDate { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Community? Community { get; set; }

    public virtual User? User { get; set; }
}
