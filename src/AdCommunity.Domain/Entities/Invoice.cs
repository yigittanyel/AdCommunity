namespace AdCommunity.Domain.Entities;

public partial class Invoice
{
    public int Id { get; set; }

    public decimal? Amount { get; set; }

    public string? Invoiceno { get; set; }

    public DateTime? Createddate { get; set; }
}
