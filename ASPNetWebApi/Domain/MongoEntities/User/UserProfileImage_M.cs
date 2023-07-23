using Microsoft.AspNetCore.Http;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace Domain.MongoEntities.User;

public class UserProfileImage_M
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid ImageId { get; set; }

    [BsonIgnoreIfNull]
    [BsonIgnoreIfDefault]
    public AppUser_M? AppUser { get; set; }

    [BsonIgnoreIfNull]
    [BsonIgnoreIfDefault]
    public Guid AppUserId { get; set; }

    public string? FileName { get; set; }
    public string? ImagePath { get; set; }

    [BsonIgnore] public IFormFile? FormImage { get; set; }
}