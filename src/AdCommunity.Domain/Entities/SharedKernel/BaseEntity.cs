using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace AdCommunity.Domain.Entities.SharedKernel;

public abstract class BaseEntity
{
    [BsonId]
    public int Id { get; set; }
    public DateTime? CreatedOn { get; protected set; }
}
