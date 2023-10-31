namespace AdCommunity.Domain.Entities;

public partial class Ticket
{
    public int Id { get; set; }

    public string? Pnr { get; set; }

    public int? Userid { get; set; }

    public decimal? Price { get; set; }

    public virtual User? User { get; set; }
}
