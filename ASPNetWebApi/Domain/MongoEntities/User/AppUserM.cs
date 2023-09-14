namespace Domain.MongoEntities.User;

public class AppUserM
{
    [BsonId]
    [BsonRepresentation(BsonType.String)]
    public Guid Id { get; set; }

    public string Username { get; set; } = string.Empty;
    public string Surname { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public byte[]? PasswordHash { get; set; }
    public byte[]? PasswordSalt { get; set; }
    public string RefreshToken { get; set; } = string.Empty;
    public DateTime TokenCreated { get; set; }
    public DateTime TokenExpires { get; set; }
    public DateTime AccountCreated { get; set; }
    public DateTime AccountLastTimeEdited { get; set; }
    public DateTime LastTimeOnline { get; set; }
    public string? ProfileImagePath { get; set; }
    public ICollection<UserProfileImageM> UserProfileImages { get; set; } = new List<UserProfileImageM>();
}