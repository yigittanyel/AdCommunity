namespace AdCommunity.Application.DTOs.TicketTypes;

public class TicketTypesBaseDto
{
    public int CommunityEventId { get; set; }
    public int CommunityId { get; set; }
    public decimal? Price { get; set; }

    public TicketTypesBaseDto()
    { 
    }
}

public class TicketTypesDto : TicketTypesBaseDto
{
    public int Id { get; set; }

    public TicketTypesDto()
    {
    }
}

public class TicketTypesCreateDto : TicketTypesBaseDto
{
    public TicketTypesCreateDto()
    {
    }
}

public class TicketTypesUpdateDto : TicketTypesBaseDto
{
    public int Id { get; set; }

    public TicketTypesUpdateDto()
    {

    }
}
