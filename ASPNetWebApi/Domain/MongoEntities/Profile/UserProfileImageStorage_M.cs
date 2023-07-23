using Domain.MongoEntities.User;

namespace Domain.MongoEntities.Profile;

public class UserProfileImageStorage_M
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public AppUser_M? User { get; set; }
    public Guid ProfileImageId { get; set; }
}