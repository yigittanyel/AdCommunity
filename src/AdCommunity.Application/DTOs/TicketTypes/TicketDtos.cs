namespace AdCommunity.Application.DTOs.TicketTypes;

public class TicketTypesBaseDto
{
    public decimal? Price { get; set; }

    public TicketTypesBaseDto()
    { 
    }
}

public class TicketTypesDto : TicketTypesBaseDto
{
    public int Id { get; set; }
    public Domain.Entities.Aggregates.Community.Community Community { get; set; }
    public Domain.Entities.Aggregates.Community.Event Event { get; set; }

    public TicketTypesDto()
    {
    }
}

public class TicketTypesCreateDto : TicketTypesBaseDto
{
    public int CommunityEventId { get; set; }
    public int CommunityId { get; set; }
    public TicketTypesCreateDto()
    {
    }
}

public class TicketTypesUpdateDto : TicketTypesBaseDto
{
    public int Id { get; set; }
    public int CommunityEventId { get; set; }
    public int CommunityId { get; set; }
    public TicketTypesUpdateDto()
    {

    }
}
