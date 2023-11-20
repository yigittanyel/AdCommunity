namespace AdCommunity.Application.DTOs.UserEvent;

public class UserEventBaseDto
{
    public int UserId { get; set; }
    public int EventId { get; set; }

    public UserEventBaseDto(int userId, int eventId)
    {
        UserId = userId;
        EventId = eventId;
    }
    public UserEventBaseDto()
    {
    }
}

public class UserEventDto : UserEventBaseDto
{
    public int Id { get; set; }

    public UserEventDto(int id)
    {
        Id = id;
    }

    public UserEventDto()
    {

    }
}

public class UserEventCreateDto : UserEventBaseDto
{
    public UserEventCreateDto()
    {

    }
}

public class UserEventUpdateDto : UserEventBaseDto
{
    public int Id { get; set; }

    public UserEventUpdateDto(int id)
    {
        Id = id;
    }
    public UserEventUpdateDto()
    {

    }
}
