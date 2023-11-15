namespace AdCommunity.Application.DTOs.Event;

public class EventBaseDto
{
    public string EventName { get; set; }
    public string Description { get; set; }
    public DateTime EventDate { get; set; }
    public string Location { get; set; }

    public EventBaseDto()
    {   
    }
}

public class EventDto : EventBaseDto
{
    public int Id { get; set; }
    public Domain.Entities.Aggregates.Community.Community Community { get; set; }


    public EventDto()
    {     
    }
}

public class EventCreateDto : EventBaseDto
{
    public int CommunityId { get; set; }

    public EventCreateDto()
    {
    }
}

public class EventUpdateDto : EventBaseDto
{
    public int Id { get; set; }
    public int CommunityId { get; set; }


    public EventUpdateDto()
    {

    }
}

