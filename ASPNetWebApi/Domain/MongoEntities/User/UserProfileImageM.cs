using Microsoft.AspNetCore.Http;

namespace Domain.MongoEntities.User;

public class UserProfileImageM
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid ImageId { get; set; }

    [BsonIgnoreIfNull]
    [BsonIgnoreIfDefault]
    public AppUserM? AppUser { get; set; }

    [BsonIgnoreIfNull]
    [BsonIgnoreIfDefault]
    public Guid AppUserId { get; set; }

    public string? FileName { get; set; }
    public string? ImagePath { get; set; }

    [BsonIgnore] public IFormFile? FormImage { get; set; }
}