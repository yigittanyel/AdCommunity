namespace AdCommunity.Application.DTOs.UserTicket;

public class UserTicketBaseDto
{
    public int UserId { get; set; }
    public int TicketId { get; set; }
    public string? Pnr { get; set; }

    public UserTicketBaseDto(int userId, int ticketId, string? pnr)
    {
        UserId = userId;
        TicketId = ticketId;
        Pnr = pnr;
    }
    public UserTicketBaseDto()
    {
    }
}

public class UserTicketDto : UserTicketBaseDto
{
    public int Id { get; set; }

    public UserTicketDto(int id)
    {
        Id = id;
    }

    public UserTicketDto()
    {

    }
}

public class UserTicketCreateDto : UserTicketBaseDto
{
    public UserTicketCreateDto()
    {

    }
}

public class UserTicketUpdateDto : UserTicketBaseDto
{
    public int Id { get; set; }

    public UserTicketUpdateDto(int id)
    {
        Id = id;
    }
    public UserTicketUpdateDto()
    {

    }
}
