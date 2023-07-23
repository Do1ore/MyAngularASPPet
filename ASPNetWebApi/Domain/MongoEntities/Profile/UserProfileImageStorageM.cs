using Domain.MongoEntities.User;

namespace Domain.MongoEntities.Profile;

public class UserProfileImageStorageM
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUserM? User { get; set; }
    public Guid ProfileImageId { get; set; }
}