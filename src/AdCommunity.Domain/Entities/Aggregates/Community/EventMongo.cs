using AdCommunity.Domain.Entities.SharedKernel;

namespace AdCommunity.Domain.Entities.Aggregates.Community;

public class EventMongo : MongoBaseEntity
{
    public string EventName { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime EventDate { get; set; }
    public string Location { get; set; } = null!;

    public EventMongo() { }

    public EventMongo(string eventName, string description, DateTime eventDate, string location)
    {
        EventName = eventName;
        Description = description;
        EventDate = eventDate;
        Location = location;
    }
}
