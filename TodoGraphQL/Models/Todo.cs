using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace TodoGraphQL.Models;

public class Todo
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } = ObjectId.GenerateNewId().ToString();

    public string Title { get; set; } = string.Empty;
    public bool IsCompleted { get; set; }
    public string UserId { get; set; } = string.Empty;
}