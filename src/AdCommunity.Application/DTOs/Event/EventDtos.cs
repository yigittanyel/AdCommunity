using AdCommunity.Application.DTOs.Community;

namespace AdCommunity.Application.DTOs.Event;

public class EventBaseDto
{
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }
    public int CommunityId { get; set; }

    public EventBaseDto()
    {   
    }
}

public class EventDto : EventBaseDto
{
    public int Id { get; set; }

    public EventDto()
    {     
    }
}

public class EventCreateDto : EventBaseDto
{
    public EventCreateDto()
    {
    }
}

public class EventUpdateDto : EventBaseDto
{
    public int Id { get; set; }

    public EventUpdateDto()
    {

    }
}

