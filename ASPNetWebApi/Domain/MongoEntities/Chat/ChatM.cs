using Domain.MongoEntities.User;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.MongoEntities.Chat;

public class ChatM
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string? Name { get; set; }
    public string? LastMessage { get; set; }

    [BsonRepresentation(BsonType.String)] public Guid? ChatAdministrator { get; set; }

    public ICollection<ChatMessageM> Messages { get; set; } = new List<ChatMessageM>();
    public ICollection<Guid> AppUserIds { get; set; } = new List<Guid>();
}