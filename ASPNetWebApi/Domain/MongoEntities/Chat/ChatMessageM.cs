using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.MongoEntities.Chat;

public sealed class ChatMessageM
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string? Content { get; set; }

    [BsonRepresentation(BsonType.DateTime)]
    public DateTime SentAt { get; set; }

    [BsonRepresentation(BsonType.String)] public Guid SenderId { get; set; }
    [BsonRepresentation(BsonType.String)] public Guid ChatId { get; set; }
}