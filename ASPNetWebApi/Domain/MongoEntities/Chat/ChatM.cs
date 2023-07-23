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

    [BsonIgnore] public List<Guid>? UserIds { get; set; } = new List<Guid>();
    public ICollection<ChatMessageM> Messages { get; set; } = new List<ChatMessageM>();
    public ICollection<AppUserM> ChatUsers { get; set; } = new List<AppUserM>();
}