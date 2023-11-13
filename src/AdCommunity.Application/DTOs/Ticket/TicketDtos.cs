namespace AdCommunity.Application.DTOs.Ticket;

public class TicketBaseDto
{
    public int CommunityEventId { get; protected set; }
    public int CommunityId { get; protected set; }
    public decimal? Price { get; protected set; }

    public TicketBaseDto()
    { 
    }
}

public class TicketDto : TicketBaseDto
{
    public int Id { get; set; }

    public TicketDto()
    {
    }
}

public class TicketCreateDto : TicketBaseDto
{
    public TicketCreateDto()
    {
    }
}

public class TicketUpdateDto : TicketBaseDto
{
    public int Id { get; set; }

    public TicketUpdateDto()
    {

    }
}
