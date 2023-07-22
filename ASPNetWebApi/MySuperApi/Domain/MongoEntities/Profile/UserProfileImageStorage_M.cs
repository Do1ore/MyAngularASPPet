namespace MySuperApi.Domain.MongoEntities.Profile;

public class UserProfileImageStorage_M
{
    public Guid Id { get; set; }

    public Guid UserId { get; set; }
    public AppUser? User { get; set; }

    public Guid ProfileImageId { get; set; }

    public UserProfileImage? ProfileImage { get; set; }
}