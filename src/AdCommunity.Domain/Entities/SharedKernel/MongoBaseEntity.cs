using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdCommunity.Domain.Entities.SharedKernel;

public abstract class MongoBaseEntity
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string? Id { get; set; }
    public DateTime? CreatedOn { get; protected set; } = DateTime.UtcNow;
}
