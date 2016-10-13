using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

[BsonIgnoreExtraElements(Inherited = true)]
public class Entity
{
    [BsonRepresentation(BsonType.ObjectId)]
    public virtual string Id { get; set; }
}