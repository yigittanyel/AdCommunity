using System;
using System.Collections.Generic;

namespace AdCommunity.Domain.Entities;

public partial class UserEvent
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int EventId { get; set; }

    public DateTime? CreatedOn { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
